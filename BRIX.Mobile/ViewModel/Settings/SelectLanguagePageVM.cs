using BRIX.Mobile.Services;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Globalization;

namespace BRIX.Mobile.ViewModel.Settings
{
    public partial class SelectLanguagePageVM : ViewModelBase
    {
        private readonly ILocalizationResourceManager _localization;

        public SelectLanguagePageVM(ILocalizationResourceManager localization)
        {
            _localization = localization;
            _selectedCulture = _localization.CurrentCulture;
            _cultures = new(_localization.Cultures);
        }

        [ObservableProperty]
        private ObservableCollection<CultureInfo> _cultures;

        [ObservableProperty]
        private CultureInfo _selectedCulture;

        [RelayCommand]
        private Task SelectCulture(CultureInfo culture)
        {
            Preferences.Set(Mobile.Settings.Account.Culture, culture.Name);
            _localization.SetCulture(culture);

            return Task.CompletedTask;
        }
    }
}
