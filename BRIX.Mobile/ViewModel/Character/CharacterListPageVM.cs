using BRIX.Mobile.Models.Character;
using BRIX.Mobile.Services;
using BRIX.Mobile.View.Character;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CharacterBM = BRIX.Library.Character.Character;

namespace BRIX.Mobile.ViewModel.Character
{
    public partial class CharacterListPageVM : ViewModelBase
    {
        private readonly ICharacterService _characterService;

        public CharacterListPageVM(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [ObservableProperty]
        private ObservableCollection<CharacterModel> _characters = new();

        [RelayCommand]
        private async Task Add()
        {
            await Navigation.NavigateAsync($"/{nameof(AddOrEditCharacterPage)}");
        }

        [RelayCommand]
        private async Task Edit(CharacterModel character)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { NavigationParameters.Character, character }
            };

            await Navigation.NavigateAsync($"/{nameof(AddOrEditCharacterPage)}", parameters);
        }

        [RelayCommand]
        private async Task Remove(CharacterModel character)
        {
            //TODO: вывести предупредительное сообщение
            await _characterService.RemoveAsync(character.Id);
            Characters.Remove(character);
        }

        [RelayCommand]
        private async Task Clear()
        {
            //TODO: вывести предупредительное сообщение
            await _characterService.RemoveAllAsync();
            Characters.Clear();
        }

        public override async Task OnNavigatedAsync()
        {
            _characters.Clear();
            List<CharacterBM> characters = await _characterService.GetAllAsync();
            Characters = new(characters.Select(character => new CharacterModel(character)));
        }
    }
}
