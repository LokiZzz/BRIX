using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Characters;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CharacterBM = BRIX.Library.Characters.Character;

namespace BRIX.Mobile.ViewModel.Characters
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
            await Navigation.NavigateAsync<AddOrEditCharacterPage>();
        }

        [RelayCommand]
        private async Task Edit(CharacterModel character)
        {
            await Navigation.NavigateAsync<AddOrEditCharacterPage>((NavigationParameters.Character, character));
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
