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

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class CharacterListPageVM : ViewModelBase
    {
        private readonly ICharacterService _characterService;
        private readonly ILocalizationResourceManager _localization;

        public CharacterListPageVM(ICharacterService characterService, ILocalizationResourceManager localization)
        {
            _characterService = characterService;
            _localization = localization;
        }

        [ObservableProperty]
        private ObservableCollection<CharacterModel> _characters = new();

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
            await Navigation.Back();
        }

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
            QuestionPopupResult result = await ShowPopupAsync<QuestionPopup, QuestionPopupResult, QuestionPopupParameters>(
                new QuestionPopupParameters(
                    title: _localization[LocalizationKeys.Warning].ToString(),
                    message: _localization[LocalizationKeys.DeleteCharacterQuestion].ToString(),
                    yesText: _localization[LocalizationKeys.Yes].ToString(),
                    noText: _localization[LocalizationKeys.No].ToString()
                )
            );

            if (result?.Answer == EQuestionPopupResult.Yes)
            {
                await _characterService.RemoveAsync(character.Id);
                Characters.Remove(character);
            }
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
            ShowHelp = Preferences.Get(Mobile.Settings.Help.ShowCharactersListHelp, true);
        }
    }
}
