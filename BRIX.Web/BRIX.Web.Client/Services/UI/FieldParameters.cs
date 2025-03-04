namespace BRIX.Web.Client.Services.UI
{
    public class FieldParameters
    {
        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string Label { get; set; } = string.Empty;

        public string InitialValue { get; set; } = string.Empty;

        public Action<FieldResult>? Callback { get; set; }
    }
}
