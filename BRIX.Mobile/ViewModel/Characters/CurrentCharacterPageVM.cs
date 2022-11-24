using BRIX.Library.Characters;
using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Characters;
using BRIX.Mobile.View.Popups;
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

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class CurrentCharacterPageVM : ViewModelBase
    {
        private readonly ICharacterService _characterService;

        public CurrentCharacterPageVM(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [ObservableProperty]
        private CharacterModel _character;

        [ObservableProperty]
        private bool _playerHaveCharacter;

        [ObservableProperty]
        private ObservableCollection<string> _exp;

        [RelayCommand]
        public async Task Create()
        {
            await Navigation.NavigateAsync<AddOrEditCharacterPage>();
        }

        [RelayCommand]
        public async Task Edit()
        {
            await Navigation.NavigateAsync<AddOrEditCharacterPage>((NavigationParameters.Character, Character));
        }

        [RelayCommand]
        public async Task Switch()
        {
            await Navigation.NavigateAsync(nameof(CharacterListPage));
        }

        [RelayCommand]
        public async Task EditHealth()
        {
            NumericEditorResult result = await ShowPopupAsync<NumericEditorPopup, NumericEditorResult>();

            if(result != null)
            {
                int newHealthValue = Character.CurrentHealth;

                switch(result.Action)
                {
                    case ENumericEditorResult.Add:
                        newHealthValue += result.EnteredValue;
                        break;
                    case ENumericEditorResult.Set:
                        newHealthValue = result.EnteredValue;
                        break;
                    case ENumericEditorResult.Substract:
                        newHealthValue -= result.EnteredValue;
                        break;
                }

                if(newHealthValue > Character.CurrentHealth)
                {
                    Character.CurrentHealth = Character.MaxHealth;
                }
                else if(newHealthValue < 0)
                {
                    Character.CurrentHealth = 0;
                }
                else
                {
                    Character.CurrentHealth = newHealthValue;
                }
            }
        }

        public override async Task OnNavigatedAsync()
        {
            List<Character> characters = await _characterService.GetAllAsync();
            PlayerHaveCharacter = characters.Any();

            if (PlayerHaveCharacter)
            {
                Character = new CharacterModel(characters.FirstOrDefault());
            }

            Exp = new ObservableCollection<string>(new List<string> { "1", "2" });
        }
    }
}
