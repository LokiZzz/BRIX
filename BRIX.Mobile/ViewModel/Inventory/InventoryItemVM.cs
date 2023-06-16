using BRIX.Library.Characters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Inventory
{
    public partial class InventoryItemVM : ObservableObject
    {
        public Color BackgroundColor { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ImageSource Icon { get; set; }

        private EInventoryItemType _type;
        public EInventoryItemType Type
        {
            get => _type;
            set
            {
                SetProperty(ref _type, value);
                OnPropertyChanged(nameof(ShowCount));
                OnPropertyChanged(nameof(ShowPrice));
                OnPropertyChanged(nameof(ShowPayload));
            }
        }

        public ObservableCollection<InventoryItemVM> Payload { get; set; } = new();

        public bool ShowPayload => Type == EInventoryItemType.Container;

        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                SetProperty(ref _count, value);
                OnPropertyChanged(nameof(ShowCount));
            }
        }

        public bool ShowCount => Count != 1 || Type == EInventoryItemType.Consumable;


        private int _price;
        public int Price
        {
            get => _price;
            set
            {
                SetProperty(ref _price, value);
                OnPropertyChanged(nameof(ShowPrice));
            }
        }

        public bool ShowPrice => Type == EInventoryItemType.Equipment || Type == EInventoryItemType.Consumable;

        private RelayCommand _descriptionCommand;
        public RelayCommand DescriptionCommand
        {
            get => _descriptionCommand;
            set => SetProperty(ref _descriptionCommand, value);
        }

        public InventoryItem OriginalModelReference { get; set; }
    }

    public enum EInventoryItemType
    {
        Thing,
        Container,
        Equipment,
        Consumable
    }
}
