using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Characters;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using CharacterBM = BRIX.Library.Characters.Character;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class AddOrEditCharacterPageVM : ViewModelBase, IQueryAttributable
    {
        private readonly ICharacterService _characterService;

        public AddOrEditCharacterPageVM(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [ObservableProperty]
        private CharacterModel _character;

        [RelayCommand]
        public async Task Save()
        {
            if (Character.Id != default)
            {
                await _characterService.UpdateAsync(Character.InternalModel);
            }
            else
            {
                Character.Id = Guid.NewGuid();
                await _characterService.AddAsync(Character.InternalModel);
            }

            await Navigation.Back();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Character = query.GetParameterOrDefault<CharacterModel>(NavigationParameters.Character)
                ?? new CharacterModel();
            query.Clear();
        }
    }
}
