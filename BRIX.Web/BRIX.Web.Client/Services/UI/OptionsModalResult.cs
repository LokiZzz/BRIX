namespace BRIX.Web.Client.Services.UI
{
    public class OptionsModalResult
    {
        public List<ModalOption>? SelectedItems { get; set; }

        public T? GetSingleOrDefault<T>() where T : class
        {
            return SelectedItems
                ?.FirstOrDefault()
                ?.GetValueOrDefault<T>() ?? default;
        }

        public List<T>? GetAll<T>() where T : class
        {
            return SelectedItems
                ?.Select(x => x.GetValueOrDefault<T>())
                ?.Where(x => x is not null)
                .Cast<T>()
                ?.ToList();
        }
    }
}