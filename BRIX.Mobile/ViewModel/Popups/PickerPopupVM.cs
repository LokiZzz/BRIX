using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Input;
using System.Collections;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Popups
{
    public partial class PickerPopupVM : ParametrizedPopupVMBase<PickerPopupParameters>
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private ObservableCollection<PickerItemVM> _items;
        public ObservableCollection<PickerItemVM> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private ObservableCollection<PickerItemVM> _selectedItems;
        public ObservableCollection<PickerItemVM> SelectedItems
        {
            get => _selectedItems;
            set => SetProperty(ref _selectedItems, value);
        }

        private PickerItemVM _selectedItem;
        public PickerItemVM SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        [RelayCommand]
        public void SelectItem()
        {
            if(!_parametersHandled)
            {
                return;
            }

            if (!Parameters.SelectMultiple)
            {
                View.Close(new PickerPopupResult 
                { 
                    SelectedItems = new List<object>() { SelectedItem.Item } 
                });
            }
        }

        private bool _parametersHandled;

        protected override void HandleParameters()
        {
            Items = new(
                Parameters.Items.Select(x => new PickerItemVM { Item = x, Text = x.ToString() })
            );

            SelectedItems = new( Items.Where(x => Parameters.SelectedItems.Contains(x.Item)) );
            SelectedItem = SelectedItems?.FirstOrDefault();
            
            Title = Parameters.Title;

            _parametersHandled = true;
        }
    }

    public class PickerPopupParameters
    {
        public List<object> Items { get; set; }
        public List<object> SelectedItems { get; set; } = new();
        public bool SelectMultiple { get; set; } = false;
        public string Title { get; set; }
    }

    public class PickerPopupResult
    {
        public List<object> SelectedItems { get; set; } = new();
        public object SelectedItem => SelectedItems.FirstOrDefault();
    }

    public class PickerItemVM
    {
        public object Item { get; set; }
        public string Text { get; set; }
    }
}
