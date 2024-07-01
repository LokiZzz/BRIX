using BRIX.Mobile.Resources.Controls;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.View.Settings;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Settings
{
    public partial class LogPageVM() : ViewModelBase
    {
        private string _log = string.Empty;
        public string Log
        {
            get => _log;
            set => SetProperty(ref _log, value);
        }

        [RelayCommand]
        public async Task Copy()
        {
            await Clipboard.Default.SetTextAsync(Log);
            await Alert(Localization.LogWasCopiedSuccessfully);
        }

        [RelayCommand]
        public void Clear()
        {
            Logger.ClearLog();
            Log = string.Empty;
        }

        public override Task OnNavigatedAsync()
        {
            Log = Logger.GetLog();

            return base.OnNavigatedAsync();
        }
    }
}
