using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BRIX.Mobile.Resources.Localizations;


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

        [ObservableProperty]
        private string _title;

        [RelayCommand]
        public async Task Save()
        {
            EEditingMode mode = Character.Id == default ? EEditingMode.Add : EEditingMode.Edit;

            switch(mode)
            {
                case EEditingMode.Add:
                    await _characterService.AddAsync(Character.InternalModel);
                    break;
                case EEditingMode.Edit:
                    await _characterService.UpdateAsync(Character.InternalModel);
                    break;
            }

            await Navigation.Back(
                stepsBack: 1,
                (NavigationParameters.Character, Character),
                (NavigationParameters.EditMode, mode)
            );
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Character = query.GetParameterOrDefault<CharacterModel>(NavigationParameters.Character)
                ?? new CharacterModel();
            query.Clear();
        }

        public override Task OnNavigatedAsync()
        {
            Title = Character.Id == default
                ? Localization.AddOrEditAbilityPageTitle_Add
                : Localization.AddOrEditAbilityPageTitle_Edit;

            return Task.CompletedTask;
        }
    }
}
