using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Popups
{
    public partial class DiceValuePopupVM : ParametrizedPopupVMBase<QuestionPopupParameters>
    {
        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private string _message;

        [ObservableProperty]
        private string _yes;

        [ObservableProperty]
        private string _no;

        [RelayCommand]
        private void FireYes()
        {
            View.Close(new QuestionPopupResult { Answer = EQuestionPopupResult.Yes });
        }

        [RelayCommand]
        private void FireNo()
        {
            View.Close(new QuestionPopupResult { Answer = EQuestionPopupResult.No });
        }

        protected override void HandleParameters()
        {
            Title = Parameters.Title;
            Message = Parameters.Message;
            Yes = Parameters.YesText;
            No = Parameters.NoText;
        }
    }

    public class QuestionPopupParameters
    {
        public QuestionPopupParameters(string title, string message, string yesText, string noText)
        {
            Title = title;
            Message = message;
            YesText = yesText;
            NoText = noText;
        }

        public string Title { get; set; }
        public string Message { get; set; }
        public string YesText { get; set; }
        public string NoText { get; set; }
    }

    public enum EQuestionPopupResult
    {
        None = 0,
        Yes = 1,
        No = 2
    }

    public class QuestionPopupResult
    {
        public EQuestionPopupResult Answer { get; set; }
    }
}
