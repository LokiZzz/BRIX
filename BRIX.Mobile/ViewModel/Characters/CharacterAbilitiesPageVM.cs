using BRIX.Library;
using BRIX.Library.Characters;
using BRIX.Mobile.Services;
using BRIX.Mobile.View.Abilities;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Abilities;
using BRIX.Mobile.Models.Abilities;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class CharacterAbilitiesPageVM : ViewModelBase, IQueryAttributable
    {
        private readonly ICharacterService _characterService;
        private readonly ILocalizationResourceManager _localization;

        private Character _currentCharacter; 

        public CharacterAbilitiesPageVM(ICharacterService characterService, ILocalizationResourceManager localization)
        {
            _characterService = characterService;
            _localization = localization;
        }

        [ObservableProperty]
        private bool _showHelp;

        [ObservableProperty]
        private ObservableCollection<AbilityModel> _abilities;

        [RelayCommand]
        private void HideHelp()
        {
            ShowHelp = false;
            Preferences.Set(Mobile.Settings.Help.ShowAbilitiesListHelp, false);
        }

        [RelayCommand]
        private async void Add()
        {
            await Navigation.NavigateAsync<AddOrEditAbilityPage>(
                (NavigationParameters.EditMode, EAbilityEditMode.Add)
            );
        }

        [RelayCommand]
        private async void Edit(AbilityModel ability)
        {
            await Navigation.NavigateAsync<AddOrEditAbilityPage>(
                (NavigationParameters.Ability, ability),
                (NavigationParameters.EditMode, EAbilityEditMode.Edit)
            );
        }

        [RelayCommand]
        private async Task Remove(AbilityModel ability)
        {
            QuestionPopupResult result = await ShowPopupAsync<QuestionPopup, QuestionPopupResult, QuestionPopupParameters>(
                new QuestionPopupParameters(
                    title: _localization[LocalizationKeys.Warning].ToString(),
                    message: _localization[LocalizationKeys.DeleteAbilityQuestion].ToString(),
                    yesText: _localization[LocalizationKeys.Yes].ToString(),
                    noText: _localization[LocalizationKeys.No].ToString()
                )
            );

            if (result?.Answer == EQuestionPopupResult.Yes)
            {
                Abilities.Remove(ability);
                _currentCharacter.Abilities.Remove(ability.InternalModel);
                await _characterService.UpdateAsync(_currentCharacter);
            }
        }

        public override async Task OnNavigatedAsync()
        {
            Character currentCharacter = await _characterService.GetCurrentCharacter();

            if (currentCharacter != null)
            {
                Abilities = new(currentCharacter.Abilities.Select(x => new AbilityModel(x)));
                _currentCharacter = currentCharacter;
            }

            ShowHelp = Preferences.Get(Mobile.Settings.Help.ShowAbilitiesListHelp, true);
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            await HandleBackFromEditing(query);
            query.Clear();
        }

        private async Task HandleBackFromEditing(IDictionary<string, object> query)
        {
            AbilityModel editedAbility = query.GetParameterOrDefault<AbilityModel>(NavigationParameters.Ability);

            if (editedAbility != null)
            {
                EAbilityEditMode mode = query.GetParameterOrDefault<EAbilityEditMode>(NavigationParameters.EditMode);

                switch(mode)
                {
                    case EAbilityEditMode.Add:
                        await AddAbility(editedAbility);
                        break;
                    case EAbilityEditMode.Edit:
                        await SaveAbility(editedAbility);
                        break;
                }
            }
        }

        private async Task AddAbility(AbilityModel ability)
        {
            _currentCharacter.Abilities.Add(ability.InternalModel);
            Abilities.Add(ability);
            await _characterService.UpdateAsync(_currentCharacter);
        }

        private async Task SaveAbility(AbilityModel ability)
        {
            await _characterService.UpdateAsync(_currentCharacter);
        }
    }
}
