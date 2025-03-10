namespace BRIX.Web.Client.Services.UI
{
    public class ModalService
    {
        public event Action<bool>? OnIsBusyChanged;
        public event Action<AlertParameters>? OnAlert;
        public event Action<NumericParameters>? OnNumeric;
        public event Action<Notification>? OnNotification;
        public event Action<List<Notification>>? OnNotifications;
        public event Action<FieldParameters>? OnField;
        public event Action<OptionsModalParameters>? OnOption;

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

        public void PushNotification(Notification notification) => OnNotification?.Invoke(notification);

        public void PushNotifications(List<Notification> notifications) => OnNotifications?.Invoke(notifications);

        public void PushErrors(IEnumerable<string> errors)
        {
            List<Notification> notifications = errors.Select(x =>
                new Notification { Type = ENotificationType.Error, Message = x }
            ).ToList();

            OnNotifications?.Invoke(notifications);
        }

        public void PushNotification(ENotificationType type, string message)
        {
            OnNotification?.Invoke(new Notification { 
                Type = type,
                Message = message
            });
        }

        public void Field(FieldParameters parameters) => OnField?.Invoke(parameters);

        public void Options(OptionsModalParameters parameters) => OnOption?.Invoke(parameters);
    }
}
