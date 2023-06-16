using BRIX.Library.Characters;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Inventory
{
    public partial class AddOrEditInventoryItemPageVM : ViewModelBase, IQueryAttributable
    {
        private EEditingMode _mode;

        private Library.Characters.Inventory _inventory;

        private InventoryItem _editingItem;

        private string _title;
        public string Title
        {
            get => _title;
            set => _title = value;
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [RelayCommand]
        public async Task Save()
        {
            await Navigation.Back(stepsBack: 1, 
                (NavigationParameters.Inventory, _inventory)
            );
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);
            _inventory = query.GetParameterOrDefault<Library.Characters.Inventory>(NavigationParameters.Inventory);
            _editingItem = query.GetParameterOrDefault<InventoryItem>(NavigationParameters.InventoryItem);

            InitializeTitle();

            if (_editingItem != null)
            {
                Name = _editingItem.Name;
            }

            query.Clear();
        }

        private void InitializeTitle()
        {
            switch(_mode)
            {
                case EEditingMode.Add:
                    Title = Localization.AddInventoryItem; break;
                case EEditingMode.Edit:
                    Title = Localization.EditInventoryItem; break;
            }
        }
    }
}