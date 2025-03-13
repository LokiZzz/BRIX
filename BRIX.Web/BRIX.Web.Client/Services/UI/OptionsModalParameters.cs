namespace BRIX.Web.Client.Services.UI
{
    public class OptionsModalParameters
    {
        public required string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public bool MultipleSelect { get; set; }

        public required List<ModalOption> Options { get; set; } = [];

        public required Action<OptionsModalResult> Callback { get; set; }
    }
}
