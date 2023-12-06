using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using System.Collections;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Popups
{
    public partial class PickerPopupVM : ParametrizedPopupVMBase<PickerPopupParameters>
    {
        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private SelectionMode _mode;
        public SelectionMode Mode
        {
            get => _mode;
            set => SetProperty(ref _mode, value);
        }

        private bool _showOk;
        public bool ShowOk
        {
            get => _showOk;
            set => SetProperty(ref _showOk, value);
        }

        private ObservableCollection<PickerItemVM> _items = [];
        public ObservableCollection<PickerItemVM> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private ObservableCollection<PickerItemVM> _selectedItems = [];
        public ObservableCollection<PickerItemVM> SelectedItems
        {
            get => _selectedItems;
            set => SetProperty(ref _selectedItems, value);
        }

        private PickerItemVM? _selectedItem;
        public PickerItemVM? SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        private bool _showListEmptyMessage;
        public bool ShowListEmptyMessage
        {
            get => _showListEmptyMessage;
            set => SetProperty(ref _showListEmptyMessage, value);
        }

        [RelayCommand]
        public void SelectItem()
        {
            if(!_parametersHandled)
            {
                return;
            }

            if (Mode == SelectionMode.Single)
            {
                if (SelectedItem?.Item != null)
                {
                    View?.Close(new PickerPopupResult { SelectedItems = new List<object>() { SelectedItem.Item } });
                }
                else
                {
                    View?.Close();
                }
            }
        }

        [RelayCommand]
        public void Ok()
        {
            View?.Close(new PickerPopupResult
            {
                SelectedItems = SelectedItems.Select(x => x.Item).Cast<object>().ToList()
            });
        }

        private bool _parametersHandled;

        protected override void HandleParameters()
        {
            if(Parameters == null)
            {
                return;
            }

            Items = new(
                Parameters.Items.Select(x => new PickerItemVM { Item = x, Text = x.ToString() ?? string.Empty })
            );

            SelectedItems = new( Items.Where(x => x.Item != null && Parameters.SelectedItems.Contains(x.Item)) );
            SelectedItem = SelectedItems?.FirstOrDefault();
            
            Title = Parameters.Title;

            Mode = Parameters.SelectMultiple ? SelectionMode.Multiple : SelectionMode.Single;
            ShowOk = Mode == SelectionMode.Multiple;
            ShowListEmptyMessage = !Items.Any();

            _parametersHandled = true;
        }
    }

    public class PickerPopupParameters
    {
        public List<object> Items { get; set; } = [];
        public List<object> SelectedItems { get; set; } = [];
        public bool SelectMultiple { get; set; } = false;
        public string Title { get; set; } = string.Empty;
    }

    public class PickerPopupResult
    {
        public List<object> SelectedItems { get; set; } = new();
        public object? SelectedItem => SelectedItems.FirstOrDefault();
    }

    public class PickerItemVM
    {
        public object? Item { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
