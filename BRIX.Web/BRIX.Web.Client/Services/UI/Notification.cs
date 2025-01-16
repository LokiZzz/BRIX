namespace BRIX.Web.Client.Services.UI
{
    public class Notification
    {
        public ENotificationType Type { get; set; }

        public string Message { get; set; } = string.Empty;
    }

    public enum ENotificationType
    {
        Info = 0,
        Success = 1,
        Warning = 2,
        Error = 3,
    }
}
