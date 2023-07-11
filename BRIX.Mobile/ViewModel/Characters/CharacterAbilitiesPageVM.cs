using BRIX.Library.Characters;
using BRIX.Mobile.Services;
using BRIX.Mobile.View.Abilities;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.Services.Navigation;
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

        private CharacterModel _character;
        public CharacterModel Character
        {
            get => _character;
            set => SetProperty(ref _character, value);
        }


        private bool _showHelp;
        public bool ShowHelp
        {
            get => _showHelp;
            set => SetProperty(ref _showHelp, value);
        }

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
        private async void Edit(CharacterAbilityModel ability)
        {
            await Navigation.NavigateAsync<AddOrEditAbilityPage>(
                (NavigationParameters.Ability, ability.Copy()),
                (NavigationParameters.EditMode, EEditingMode.Edit)
            );
        }

        [RelayCommand]
        private async Task Remove(CharacterAbilityModel ability)
        {
            AlertPopupResult result = await Ask(Localization.DeleteAbilityQuestion);

            if (result?.Answer == EAlertPopupResult.Yes)
            {
                Character.RemoveAbility(ability.InternalModel.Id);
                await _characterService.UpdateAsync(Character.InternalModel);
            }
        }

        public override async Task OnNavigatedAsync()
        {
            await Initialize();

            ShowHelp = Preferences.Get(Mobile.Settings.Help.ShowAbilitiesListHelp, true);
        }

        private async Task Initialize(bool force = false)
        {
            if (Character == null || force)
            {
                Character currentCharacter = await _characterService.GetCurrentCharacter();

                if (currentCharacter != null)
                {
                    Character = new CharacterModel(currentCharacter);
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
            CharacterAbilityModel editedAbility = query.GetParameterOrDefault<CharacterAbilityModel>(NavigationParameters.Ability);

            if (editedAbility != null)
            {
                EEditingMode mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);

                switch(mode)
                {
                    case EEditingMode.Add:
                        Character.AddAbility(editedAbility);
                        break;
                    case EEditingMode.Edit:
                        Character.UpdateAbility(editedAbility);
                        break;
                }

                await _characterService.UpdateAsync(Character.InternalModel);
            }
        }
    }
}
