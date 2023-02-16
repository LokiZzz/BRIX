using BRIX.Mobile.Services;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Settings
{
    public partial class SelectLanguagePopupVM : PopupVMBase
    {
        private readonly ILocalizationResourceManager _localization;

        public SelectLanguagePopupVM(ILocalizationResourceManager localization)
        {
            _localization = localization;
            _cultures = new(_localization.Cultures.Select(x => new CultureInfoVM { CultureInfo = x }));
        }

        [ObservableProperty]
        private ObservableCollection<CultureInfoVM> _cultures;

        [RelayCommand]
        private void SelectCulture(CultureInfoVM culture)
        {
            Preferences.Set(Mobile.Settings.Account.Culture, culture.CultureInfo.Name);
            _localization.SetCulture(culture.CultureInfo);

            View.Close();
        }
    }

    public class CultureInfoVM
    {
        public CultureInfo CultureInfo { get; set; }

        public string LanguageNativeName => CultureInfo == null ? null : CultureInfo.NativeName.Capitalize();
    }
}
