using BRIX.Library;
using BRIX.Library.Characters;
using BRIX.Mobile.Services;
using BRIX.Mobile.View.Abilities;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class CharacterAbilitiesPageVM : ViewModelBase
    {
        private readonly ICharacterService _characterService;

        public CharacterAbilitiesPageVM(ICharacterService characterService)
        {
            _characterService = characterService;
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
            await Navigation.NavigateAsync<AddOrEditAbilityPage>();
        }

        [ObservableProperty]
        private ObservableCollection<Ability> _abilities;

        public override async Task OnNavigatedAsync()
        {
            Character currentCharacter = await _characterService.GetCurrentCharacter();

            if (currentCharacter != null)
            {
                Abilities = new ObservableCollection<Ability>(currentCharacter.Abilities)
                {
                    new Ability { Name = "Ability 1" },
                    new Ability { Name = "Ability 2" }
                };
                ShowHelp = Preferences.Get(Mobile.Settings.Help.ShowAbilitiesListHelp, true);
            }
        }
    }
}
