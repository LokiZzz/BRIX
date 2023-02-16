using BRIX.Mobile.Services;
using BRIX.Mobile.View.Settings;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Settings
{
    public partial class SettingsPageVM : ViewModelBase
    {
        private readonly ILocalizationResourceManager _localization;

        public SettingsPageVM(ILocalizationResourceManager localization)
        {
            _localization = localization;
            Initialize();
        }

        [ObservableProperty]
        public string _currentLanguage;

        [RelayCommand]
        private async Task SelectLanguage()
        {
            await ShowPopupAsync<SelectLanguagePopup>();
        }

        private void Initialize()
        {
            CurrentLanguage = _localization.CurrentCulture.NativeName.Capitalize();
        }

        public override Task OnNavigatedAsync()
        {
            Initialize();

            return base.OnNavigatedAsync();
        }
    }
}
