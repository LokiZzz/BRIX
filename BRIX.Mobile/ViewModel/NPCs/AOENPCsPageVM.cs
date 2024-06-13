using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;

namespace BRIX.Mobile.ViewModel.NPCs
{
    public partial class AOENPCsPageVM : ViewModelBase, IQueryAttributable
    {
        private EEditingMode _mode;

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            ApplyMode(query);

            query.Clear();
        }

        private void ApplyMode(IDictionary<string, object> query)
        {
            _mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);

            Title = _mode switch
            {
                EEditingMode.Add => Localization.AddNPC,
                EEditingMode.Edit => Localization.EditNPC,
                _ => string.Empty
            };
        }
    }
}
