using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Popups
{
    public partial class EntryPopupVM : ParametrizedPopupVMBase<EntryPopupParameters>
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private string _placeholder;
        public string Placeholder
        {
            get => _placeholder;
            set => SetProperty(ref _placeholder, value);
        }

        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private string _buttonText;
        public string ButtonText
        {
            get => _buttonText;
            set => SetProperty(ref _buttonText, value);
        }

        [RelayCommand]
        public void FireOk()
        {
            View.Close(new EntryPopupResult { Text = string.IsNullOrEmpty(Text) ? null : Text });
        }

        protected override void HandleParameters()
        {
            Title = Parameters.Title;
            Placeholder = Parameters.Placeholder;
            Message = Parameters.Message;
            ButtonText = Parameters.ButtonText;
        }
    }

    public class EntryPopupParameters
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Placeholder { get; set; }
        public string ButtonText { get; set; }
    }

    public class EntryPopupResult
    {
        public string Text { get; set; }
    }
}
