using BRIX.Library;
using BRIX.Library.Characters;
using BRIX.Mobile.Services;
using BRIX.Mobile.View.Abilities;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Abilities;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class CharacterAbilitiesPageVM : ViewModelBase
    {
        private readonly ICharacterService _characterService;
        private readonly ILocalizationResourceManager _localization;

        public CharacterAbilitiesPageVM(ICharacterService characterService, ILocalizationResourceManager localization)
        {
            _characterService = characterService;
            _localization = localization;
        }

        [ObservableProperty]
        private bool _showHelp;

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
        private async void Edit(Ability ability)
        {
            await Navigation.NavigateAsync<AddOrEditAbilityPage>(
                (NavigationParameters.Ability, ability),
                (NavigationParameters.EditMode, EAbilityEditMode.Edit)
            );
        }

        [RelayCommand]
        private async void Remove(Ability ability)
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
                //Delete the ability from character and local VM abilities collection
            }
        }

        [ObservableProperty]
        private ObservableCollection<Ability> _abilities;

        public override async Task OnNavigatedAsync()
        {
            Character currentCharacter = await _characterService.GetCurrentCharacter();

            if (currentCharacter != null)
            {
                Abilities = new ObservableCollection<Ability>(currentCharacter.Abilities);
            }

            ShowHelp = Preferences.Get(Mobile.Settings.Help.ShowAbilitiesListHelp, true);
        }
    }
}
