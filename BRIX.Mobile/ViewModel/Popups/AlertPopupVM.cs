using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Popups
{
    public partial class AlertPopupVM : ParametrizedPopupVMBase<AlertPopupParameters>
    {
        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private string _message;

        [ObservableProperty]
        private string _yes;

        [ObservableProperty]
        private string _no;

        [ObservableProperty]
        private string _ok;

        [ObservableProperty]
        private bool _showYes;

        [ObservableProperty]
        private bool _showNo; 
        
        [ObservableProperty]
        private bool _showOk;

        [RelayCommand]
        private void FireYes()
        {
            View.Close(new AlertPopupResult { Answer = EAlertPopupResult.Yes });
        }

        [RelayCommand]
        private void FireNo()
        {
            View.Close(new AlertPopupResult { Answer = EAlertPopupResult.No });
        }

        [RelayCommand]
        private void FireOk()
        {
            View.Close(new AlertPopupResult { Answer = EAlertPopupResult.None });
        }

        protected override void HandleParameters()
        {
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
        public string Title { get; init; }
        public string Message { get; init; }
        public string YesText { get; init; }
        public string NoText { get; init; }
        public string OkText { get; init; }
        public EAlertMode Mode { get; init; } = EAlertMode.AskYesOrNo;
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
