using BRIX.Mobile.Models.Character;
using BRIX.Mobile.Services;
using BRIX.Mobile.View.Character;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using CharacterBM = BRIX.Library.Character.Character;

namespace BRIX.Mobile.ViewModel.Character
{
    public partial class AddOrEditCharacterPageVM : ViewModelBase, IQueryAttributable
    {
        private readonly ICharacterService _characterService;

        public AddOrEditCharacterPageVM(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [ObservableProperty]
        private CharacterModel _character = new();

        [RelayCommand]
        public async Task Save(CharacterModel character)
        {
            if (character.Id != default)
            {
                await _characterService.UpdateAsync(character.Character);
            }
            else
            {
                character.Id = Guid.NewGuid();
                await _characterService.AddAsync(character.Character);
            }

            await Navigation.NavigateAsync($"..");
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey(NavigationParameters.Character))
            {
                Character = query[NavigationParameters.Character] as CharacterModel;
            }
        }
    }
}
