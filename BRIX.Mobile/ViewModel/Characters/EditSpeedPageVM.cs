using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class EditSpeedPageVM(ICharacterService characterService) : ViewModelBase, IQueryAttributable
    {
        public ICharacterService CharacterService { get; } = characterService;

        private CharacterModel? _character;
        public CharacterModel? Character
        {
            get => _character;
            set => SetProperty(ref _character, value);
        }

        [RelayCommand]
        public async Task Save()
        {
            if (Character != null)
            {
                if(Character.FreeExperience < 0)
                {
                    await Alert(Localization.NotEnoughEXPForSpeed);

                    return;
                }

                await CharacterService.UpdateAsync(Character.InternalModel);
            }

            await Navigation.Back();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            CharacterModel? character = query.GetParameterOrDefault<CharacterModel>(NavigationParameters.Character);

            if(character != null)
            {
                Character = new CharacterModel(character.InternalModel);
            }

            query.Clear();
        }
    }
}
