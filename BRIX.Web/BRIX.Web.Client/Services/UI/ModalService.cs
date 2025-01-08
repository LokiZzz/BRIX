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

        public event Action<bool>? OnIsBusyChanged;

        private void NotifyIsBusyChanged(bool isBusy) => OnIsBusyChanged?.Invoke(isBusy);
    }
}
