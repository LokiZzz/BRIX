using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Popups
{
    public partial class EntryPopupVM : ParametrizedPopupVMBase<EntryPopupParameters>
    {
        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _message = string.Empty;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private string _placeholder = string.Empty;
        public string Placeholder
        {
            get => _placeholder;
            set => SetProperty(ref _placeholder, value);
        }

        private string _text = string.Empty;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private string _buttonText = string.Empty;

        public string ButtonText
        {
            get => _buttonText;
            set => SetProperty(ref _buttonText, value);
        }


        public event EventHandler? OnEmptyValueEntered;

        [RelayCommand]
        public void FireOk()
        {
            if (string.IsNullOrEmpty(Text) && OnEmptyValueEntered != null)
            {
                OnEmptyValueEntered(this, EventArgs.Empty);
            }
            else
            {
                View?.Close(new EntryPopupResult { Text = string.IsNullOrEmpty(Text) ? string.Empty : Text });
            }
        }

        protected override void HandleParameters()
        {
            if (Parameters == null)
            {
                return;
            }

            Title = Parameters.Title;
            Placeholder = Parameters.Placeholder;
            Message = Parameters.Message;
            ButtonText = Parameters.ButtonText;
        }
    }

    public class EntryPopupParameters
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Placeholder { get; set; } = string.Empty;
        public string ButtonText { get; set; } = string.Empty;
    }

    public class EntryPopupResult
    {
        public string Text { get; set; } = string.Empty;
    }
}
