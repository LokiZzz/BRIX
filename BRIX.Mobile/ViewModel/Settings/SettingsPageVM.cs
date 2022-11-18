using BRIX.Mobile.View.Settings;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Settings
{
    public partial class SettingsPageVM : ViewModelBase
    {
        [RelayCommand]
        private Task SelectLanguage()
        {
            return Navigation.NavigateAsync<SelectLanguagePage>();
        }
    }
}
