namespace BRIX.Web.Client.Services.UI
{
    public class ModalOption
    {
        public required string Title { get; set; }

        public required object Value { get; set; }

        public bool IsSelected { get; set; }

        public T? GetValueOrDefault<T>() where T : class
        {
            return Value as T ?? default;
        }
    }
}
