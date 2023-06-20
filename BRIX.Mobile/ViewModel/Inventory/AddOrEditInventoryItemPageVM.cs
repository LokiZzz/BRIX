using BRIX.Library.Characters;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Inventory
{
    public partial class AddOrEditInventoryItemPageVM : ViewModelBase, IQueryAttributable
    {
        private readonly ILocalizationResourceManager _localization;

        private EEditingMode _mode;
        private Library.Characters.Inventory _inventory;
        private InventoryItem _editingItem;

        public AddOrEditInventoryItemPageVM(ILocalizationResourceManager localization)
        {
            _localization = localization;
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private InventoryItemVM _item;
        public InventoryItemVM Item
        {
            get => _item;
            set => SetProperty(ref _item, value);
        }

        private InventoryItemTypeVM _selectedType;
        public InventoryItemTypeVM SelectedType
        {
            get => _selectedType;
            set
            {
                SetProperty(ref _selectedType, value);
                UpdateItemType(value);
            }
        }

        private ObservableCollection<InventoryItemTypeVM> _types;
        public ObservableCollection<InventoryItemTypeVM> Types
        {
            get => _types;
            set => SetProperty(ref _types, value);
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
            InventoryItemConverter converter = new ();

            Item = _editingItem != null 
                ? converter.ToVM(_editingItem)
                : converter.ToVM(new InventoryItem());

            Types = new ObservableCollection<InventoryItemTypeVM>(
                Enum.GetValues<EInventoryItemType>().Select(x => new InventoryItemTypeVM { 
                    Text = _localization[x.ToString("G")].ToString(),
                    Type  = x
                })
            );
            SelectedType = Types.First(x => x.Type == Item.Type);
            InitializeTitle();

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

        private void UpdateItemType(InventoryItemTypeVM value)
        {
            Item.Type = value.Type;
        }
    }

    public class InventoryItemTypeVM
    {
        public EInventoryItemType Type { get; set; }
        public string Text { get; set; }

        public override string ToString() => Text;
    }
}