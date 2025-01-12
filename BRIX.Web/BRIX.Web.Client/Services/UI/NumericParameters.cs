namespace BRIX.Web.Client.Services.UI
{
    public class NumericParameters
    {
        public string Title { get; set; } = string.Empty;

        public Action<NumericResult>? Callback { get; set; }
    }
}
