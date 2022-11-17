using BRIX.Mobile.Services;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Account
{
    public partial class SelectLanguagePageVM : ViewModelBase
    {
        private readonly ILocalizationResourceManager _localization;

        public SelectLanguagePageVM(ILocalizationResourceManager localization)
        {
            _localization = localization;
        }

        [ObservableProperty]
        private ObservableCollection<CultureInfo> _cultures = new ObservableCollection<CultureInfo>
        {
            CultureInfo.GetCultureInfo("ru"),
            CultureInfo.GetCultureInfo("en")
        };

        [ObservableProperty]
        private CultureInfo _selectedCulture;

        [RelayCommand]
        private Task SelectCulture(CultureInfo culture)
        {
            Preferences.Set(Settings.Account.Culture, culture.Name);
            _localization.SetCulture(culture);

            return Task.CompletedTask;
        }
    }
}
