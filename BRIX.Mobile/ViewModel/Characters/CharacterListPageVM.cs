using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Characters;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BRIX.Mobile.Resources.Localizations;
using CharacterBM = BRIX.Library.Characters.Character;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class CharacterListPageVM(ICharacterService characterService) : ViewModelBase, IQueryAttributable
    {
        private readonly ICharacterService _characterService = characterService;
        private bool _initialized = false;

        [ObservableProperty]
        private ObservableCollection<CharacterModel> _characters = [];

        [ObservableProperty]
        private bool _showHelp;

        [RelayCommand]
        private void HideHelp()
        {
            ShowHelp = false;
            Preferences.Set(Mobile.Settings.Help.ShowCharactersListHelp, false);
        }

        [RelayCommand]
        private async Task Select(CharacterModel characterToSelect)
        {
            await _characterService.SelectCurrentCharacter(characterToSelect.InternalModel);
            WeakReferenceMessenger.Default.Send(new CurrentCharacterChanged(characterToSelect));
            await Navigation.Back();
        }

        [RelayCommand]
        private async Task Add()
        {
            await Navigation.NavigateAsync<AOECharacterPage>();
        }

        [RelayCommand]
        private async Task Edit(CharacterModel character)
        {
            await Navigation.NavigateAsync<AOECharacterPage>((NavigationParameters.Character, character));
        }

        [RelayCommand]
        private async Task Remove(CharacterModel character)
        {
            AlertPopupResult? result = await Ask(Localization.DeleteCharacterQuestion);

            if (result?.Answer == EAlertPopupResult.Yes)
            {
                await _characterService.RemoveAsync(character.Id);
                Characters.Remove(character);
            }
        }

        public override async Task OnNavigatedAsync()
        {
            if (!_initialized)
            {
                List<CharacterBM> characters = await _characterService.GetAllAsync();
                Characters = new(characters.Select(character => new CharacterModel(character)));
                _initialized = true;
            }

            ShowHelp = Preferences.Get(Mobile.Settings.Help.ShowCharactersListHelp, true);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            HandleBackFromEditing(query);
            query.Clear();
        }

        private void HandleBackFromEditing(IDictionary<string, object> query)
        {
            CharacterModel? character = query.GetParameterOrDefault<CharacterModel>(NavigationParameters.Character);

            if (character != null)
            {
                EEditingMode mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);

                if(mode == EEditingMode.Add)
                {
                    Characters.Add(character);
                }
            }
        }
    }

    public class CurrentCharacterChanged(CharacterModel character) : ValueChangedMessage<CharacterModel>(character) { }
}
