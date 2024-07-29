using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BRIX.Mobile.Resources.Localizations;
using CommunityToolkit.Mvvm.Messaging;
using System.Reflection;


namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class AOECharacterPageVM(
        ICharacterService characterService, 
        ILocalizationResourceManager localization) : ViewModelBase, IQueryAttributable
    {
        private readonly ICharacterService _characterService = characterService;
        private readonly ILocalizationResourceManager _localization = localization;

        [ObservableProperty]
        private CharacterModel _character = new(new());

        [ObservableProperty]
        private string _title = string.Empty;

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

            WeakReferenceMessenger.Default.Send(Character);

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
                ? _localization[LocalizationKeys.AddOrEditCharacterPageTitle_Add].ToString() ?? string.Empty
                : _localization[LocalizationKeys.AddOrEditCharacterPageTitle_Edit].ToString() ?? string.Empty;

            return Task.CompletedTask;
        }
    }
}
