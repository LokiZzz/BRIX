using BRIX.Library.Characters;
using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Characters;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
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

        public override async Task OnNavigatedAsync()
        {
            List<Character> characters = await _characterService.GetAllAsync();
            PlayerHaveCharacter = characters.Any();

            if (PlayerHaveCharacter)
            {
                Character = new CharacterModel(characters.FirstOrDefault());
            }
        }
    }
}
