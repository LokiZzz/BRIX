using BRIX.Mobile.Resources.Controls;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.View.Settings;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Globalization;

namespace BRIX.Mobile.ViewModel.Settings
{
    public partial class SettingsPageVM(ILocalizationResourceManager localization, ICharacterService characterService)
        : ViewModelBase
    {
        private readonly ILocalizationResourceManager _localization = localization;
        private readonly ICharacterService _characterService = characterService;
        private ObservableCollection<CultureInfoVM> _cultures = [];
        public ObservableCollection<CultureInfoVM> Cultures
        {
            get => _cultures;
            set => SetProperty(ref _cultures, value);
        }

        private CultureInfoVM? _selectedCulture;
        public CultureInfoVM? SelectedCulture
        {
            get => _selectedCulture;
            set
            {
                if(value?.CultureInfo != null && SetProperty(ref _selectedCulture, value))
                {
                    Preferences.Set(Mobile.Settings.Account.Culture, value.CultureInfo.Name);
                    _localization.SetCulture(value.CultureInfo);
                }
            }
        }

        [RelayCommand]
        public void ResetHelpCards()
        {
            List<string> helps = _localization.GetKeys()
                .Where(x => x.EndsWith("_Help"))
                .ToList();

            foreach(string help in helps)
            {
                Preferences.Set(help, true);
            }

            WeakReferenceMessenger.Default.Send<ShowHelpCardsChanged>();
        }

        [RelayCommand]
        public async Task ResetCharactersData()
        {
            Popups.AlertPopupResult? result = await Ask(Localization.AskWantToResetAllCharactersData);

            if(result?.Answer == Popups.EAlertPopupResult.Yes)
            {
                await _characterService.RemoveAllAsync();
                WeakReferenceMessenger.Default.Send(new ShowCharacterTabsChanged(false));
            }
        }

        public override Task OnNavigatedAsync()
        {
            Cultures = new(_localization.Cultures.Select(x => new CultureInfoVM { CultureInfo = x }));
            SelectedCulture = Cultures.FirstOrDefault(x => x.CultureInfo == _localization.CurrentCulture);

            return base.OnNavigatedAsync();
        }
    }

    public class CultureInfoVM
    {
        public CultureInfo? CultureInfo { get; set; }

        public string LanguageNativeName => CultureInfo == null ? string.Empty : CultureInfo.NativeName.Capitalize();

        public override string ToString() => LanguageNativeName;
    }
}
