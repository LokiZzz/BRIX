using BRIX.Library.Characters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Inventory
{
    public partial class InventoryItemVM : ObservableObject
    {
        public InventoryItemVM(InventoryItem model)
        {
            InternalModel = model;
        }

        public InventoryItem InternalModel { get; set; }

        public Color BackgroundColor { get; set; }

        public string Name
        {
            get => InternalModel.Name;
            set => SetProperty(
                InternalModel.Name, value, InternalModel, (character, prop) => character.Name = prop
            );
        }

        public string Description
        {
            get => InternalModel.Description;
            set => SetProperty(
                InternalModel.Description, value, InternalModel, (character, prop) => character.Description = prop
            );
        }

        public int Count
        {
            get => InternalModel.Count;
            set
            {
                SetProperty(
                    InternalModel.Count, value, InternalModel, (character, prop) => character.Count = prop
                );
                OnPropertyChanged(nameof(ShowCount));
            }
        }

        public int Price
        {
            get
            {
                if(Type == EInventoryItemType.Equipment || Type == EInventoryItemType.Consumable)
                {
                    return (InternalModel as MaterialSupport).CoinsPrice;
                }

                return 0;
            }
            set
            {
                if (Type == EInventoryItemType.Equipment || Type == EInventoryItemType.Consumable)
                {
                    MaterialSupport internalModel = InternalModel as MaterialSupport;
                    SetProperty(
                        internalModel.Count, value, internalModel, (character, prop) => character.Count = prop
                    );
                }

                OnPropertyChanged(nameof(ShowPrice));
            }
        }

        public ImageSource Icon { get; set; }

        private EInventoryItemType _type;
        public EInventoryItemType Type
        {
            get => _type;
            set
            {
                if(SetProperty(ref _type, value))
                {
                    InternalModel = InventoryItemConverter.CreateItemByType(value);
                }

                OnPropertyChanged(nameof(ShowCount));
                OnPropertyChanged(nameof(ShowPrice));
                OnPropertyChanged(nameof(ShowPayload));
            }
        }

        public ObservableCollection<InventoryItemVM> Payload { get; set; } = new();

        public bool ShowPayload => Type == EInventoryItemType.Container;     
        public bool ShowCount => Count != 1 || Type == EInventoryItemType.Consumable;
        public bool ShowPrice => Type == EInventoryItemType.Equipment || Type == EInventoryItemType.Consumable;

        private RelayCommand _descriptionCommand;
        public RelayCommand DescriptionCommand
        {
            get => _descriptionCommand;
            set => SetProperty(ref _descriptionCommand, value);
        }

    }

    public enum EInventoryItemType
    {
        Thing,
        Container,
        Equipment,
        Consumable
    }
}
