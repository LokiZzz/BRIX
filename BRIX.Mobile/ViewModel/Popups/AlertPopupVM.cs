using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Popups
{
    public partial class AlertPopupVM : ParametrizedPopupVMBase<AlertPopupParameters>
    {
        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private string _message = string.Empty;

        [ObservableProperty]
        private string _yes = string.Empty;

        [ObservableProperty]
        private string _no = string.Empty;

        [ObservableProperty]
        private string _ok = string.Empty;

        [ObservableProperty]
        private bool _showYes;

        [ObservableProperty]
        private bool _showNo; 
        
        [ObservableProperty]
        private bool _showOk;

        [RelayCommand]
        private void FireYes()
        {
            View?.Close(new AlertPopupResult { Answer = EAlertPopupResult.Yes });
        }

        [RelayCommand]
        private void FireNo()
        {
            View?.Close(new AlertPopupResult { Answer = EAlertPopupResult.No });
        }

        [RelayCommand]
        private void FireOk()
        {
            View?.Close(new AlertPopupResult { Answer = EAlertPopupResult.None });
        }

        protected override void HandleParameters()
        {
            if (Parameters == null)
            {
                return;
            }

            Title = Parameters.Title;
            Message = Parameters.Message;
            Yes = Parameters.YesText;
            No = Parameters.NoText;
            Ok = Parameters.OkText;

            ShowYes = Parameters.Mode == EAlertMode.AskYesOrNo;
            ShowNo = Parameters.Mode == EAlertMode.AskYesOrNo;
            ShowOk = Parameters.Mode == EAlertMode.ShowMessage;
        }
    }

    public class AlertPopupParameters
    {
        public string Title { get; init; } = Localization.Warning;
        public string Message { get; init; } = string.Empty;
        public string YesText { get; init; } = Localization.Yes;
        public string NoText { get; init; } = Localization.No;
        public string OkText { get; init; } = Localization.Ok;
        public EAlertMode Mode { get; init; } = EAlertMode.ShowMessage;
    }

    public enum EAlertMode
    {
        AskYesOrNo = 0,
        ShowMessage = 1
    }

    public enum EAlertPopupResult
    {
        None = 0,
        Yes = 1,
        No = 2
    }

    public class AlertPopupResult
    {
        public EAlertPopupResult Answer { get; set; }
    }
}
