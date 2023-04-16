using BRIX.Mobile.Services;
using BRIX.Mobile.View.Settings;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

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
        private ObservableCollection<CultureInfoVM> _cultures;

        [ObservableProperty]
        private CultureInfoVM _selectedCulture;

        partial void OnSelectedCultureChanged(CultureInfoVM value)
        {
            if (!value.CultureInfo.Equals(_localization.CurrentCulture))
            {
                Preferences.Set(Mobile.Settings.Account.Culture, value.CultureInfo.Name);
                _localization.SetCulture(value.CultureInfo);
            }
        }

        private void Initialize()
        {
            Cultures = new(_localization.Cultures.Select(x => new CultureInfoVM { CultureInfo = x }));
            //SelectedCulture = Cultures.FirstOrDefault(x => x.CultureInfo.Equals(_localization.CurrentCulture));
        }

        public override Task OnNavigatedAsync()
        {
            Initialize();

            return base.OnNavigatedAsync();
        }
    }
}
