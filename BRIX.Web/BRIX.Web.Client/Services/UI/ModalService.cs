namespace BRIX.Web.Client.Services.UI
{
    public class ModalService
    {
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

        public void Alert(AlertParameters parameters) => OnAlert?.Invoke(parameters);

        public event Action<bool>? OnIsBusyChanged;

        public event Action<AlertParameters>? OnAlert;
        
        private void NotifyIsBusyChanged(bool isBusy) => OnIsBusyChanged?.Invoke(isBusy);
    }
}
