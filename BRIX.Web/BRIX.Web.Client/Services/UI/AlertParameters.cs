namespace BRIX.Web.Client.Services.UI
{
    public class AlertParameters
    {
        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public EAlertMode Mode { get; set; }

        public Action<AlertResult>? Callback { get; set; }
    }
}
