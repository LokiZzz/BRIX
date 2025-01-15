namespace BRIX.Web.Client.Services.UI
{
    public class ModalService
    {
        public event Action<bool>? OnIsBusyChanged;
        public event Action<AlertParameters>? OnAlert;
        public event Action<NumericParameters>? OnNumeric;
        public event Action<string>? OnError;

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                NotifyIsBusyChanged(value);
            }
        }

        private void NotifyIsBusyChanged(bool isBusy) => OnIsBusyChanged?.Invoke(isBusy);

        public void Alert(AlertParameters parameters) => OnAlert?.Invoke(parameters);

        public void Numeric(NumericParameters parameters) => OnNumeric?.Invoke(parameters);

        public void PushError(string errorMessage) => OnError?.Invoke(errorMessage);
    }
}
