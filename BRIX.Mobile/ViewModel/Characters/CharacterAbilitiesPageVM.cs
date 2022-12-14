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
using CommunityToolkit.Mvvm.Messaging;

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

            WeakReferenceMessenger.Default.Register<CurrentCharacterChanged>(
                this, 
                async (r, m) => await Initialize(true)
            );
        }

        [ObservableProperty]
        private bool _showHelp;

        private bool _initialized = false;
        
        [ObservableProperty]
        private ObservableCollection<AbilityModel> _abilities = new();

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
                (NavigationParameters.EditMode, EEditingMode.Add)
            );
        }

        [RelayCommand]
        private async void Edit(AbilityModel ability)
        {
            await Navigation.NavigateAsync<AddOrEditAbilityPage>(
                (NavigationParameters.Ability, ability),
                (NavigationParameters.EditMode, EEditingMode.Edit)
            );
        }

        [RelayCommand]
        private async void Upgrade(AbilityModel ability)
        {
            await Navigation.NavigateAsync<AddOrEditAbilityPage>(
                (NavigationParameters.Ability, ability),
                (NavigationParameters.EditMode, EEditingMode.Upgrade)
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
            await Initialize();

            ShowHelp = Preferences.Get(Mobile.Settings.Help.ShowAbilitiesListHelp, true);
        }

        private async Task Initialize(bool force = false)
        {
            Character currentCharacter = await _characterService.GetCurrentCharacter();

            if (currentCharacter != null)
            {
                _currentCharacter = currentCharacter;

                if (!_initialized || force)
                {
                    Abilities = new(currentCharacter.Abilities.Select(x => new AbilityModel(x)));
                    _initialized = true;
                }
            }
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
                EEditingMode mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);

                if(mode == EEditingMode.Add)
                {
                    _currentCharacter.Abilities.Add(editedAbility.InternalModel);
                    Abilities.Add(editedAbility);
                }

                await _characterService.UpdateAsync(_currentCharacter);
            }
        }
    }
}
