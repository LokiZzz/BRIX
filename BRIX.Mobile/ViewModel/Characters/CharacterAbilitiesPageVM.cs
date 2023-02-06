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
using BRIX.Utility.Extensions;
using BRIX.Mobile.Models.Characters;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class CharacterAbilitiesPageVM : ViewModelBase, IQueryAttributable
    {
        private readonly ICharacterService _characterService;
        private readonly ILocalizationResourceManager _localization;

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
        private CharacterModel _character;

        [ObservableProperty]
        private bool _showHelp;

        private bool _initialized = false;

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
                (NavigationParameters.Ability, ability.Copy()),
                (NavigationParameters.EditMode, EEditingMode.Edit)
            );
        }

        [RelayCommand]
        private async void Upgrade(AbilityModel ability)
        {
            await Navigation.NavigateAsync<AddOrEditAbilityPage>(
                (NavigationParameters.Ability, ability.Copy()),
                (NavigationParameters.EditMode, EEditingMode.Upgrade)
            );
        }

        [RelayCommand]
        private async Task Remove(AbilityModel ability)
        {
            QuestionPopupResult result = await ShowPopupAsync<QuestionPopup, QuestionPopupResult, QuestionPopupParameters>(
                new QuestionPopupParameters
                { 
                    Title = _localization[LocalizationKeys.Warning].ToString(),
                    Message = _localization[LocalizationKeys.DeleteAbilityQuestion].ToString(),
                    YesText = _localization[LocalizationKeys.Yes].ToString(),
                    NoText = _localization[LocalizationKeys.No].ToString()
                }
            );

            if (result?.Answer == EQuestionPopupResult.Yes)
            {
                _character.RemoveAbility(ability.InternalModel.Guid);
                await _characterService.UpdateAsync(_character.InternalModel);
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
                Character = new CharacterModel(currentCharacter);

                if (!_initialized || force)
                {
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

                switch(mode)
                {
                    case EEditingMode.Add:
                        _character.AddAbility(editedAbility);
                        break;
                    case EEditingMode.Edit:
                    case EEditingMode.Upgrade:
                        _character.UpdateAbility(editedAbility);
                        break;
                }

                await _characterService.UpdateAsync(_character.InternalModel);
            }
        }
    }
}
