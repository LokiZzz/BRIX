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

        private InventoryItem _internalModel;
        public InventoryItem InternalModel
        {
            get => _internalModel;
            set
            {
                _internalModel = value;

                switch(value)
                {
                    case Container:
                        Type = EInventoryItemType.Container;
                        break;
                    case Equipment:
                        Type = EInventoryItemType.Equipment;
                        break;
                    case Consumable:
                        Type = EInventoryItemType.Consumable;
                        break;
                    case InventoryItem:
                        Type = EInventoryItemType.Thing;
                        break;
                }
            }
        }

        public string Name
        {
            get => InternalModel.Name;
            set => SetProperty(
                InternalModel.Name, value, InternalModel, (model, prop) => model.Name = prop
            );
        }

        public string Description
        {
            get => InternalModel.Description;
            set => SetProperty(
                InternalModel.Description, value, InternalModel, (model, prop) => model.Description = prop
            );
        }

        public int Count
        {
            get => InternalModel.Count;
            set
            {
                SetProperty(
                    InternalModel.Count, value, InternalModel, (model, prop) => model.Count = prop
                );
                OnPropertyChanged(nameof(ShowCount));
                OnPropertyChanged(nameof(PriceString));
                OnPropertyChanged(nameof(FullPrice));

                if (Type == EInventoryItemType.Equipment || Type == EInventoryItemType.Consumable)
                {
                    if (OnFullPriceChanged != null)
                    {
                        OnFullPriceChanged(this, FullPrice);
                    }
                }
            }
        }

        public event EventHandler<int> OnFullPriceChanged;

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
                        internalModel.CoinsPrice, value, internalModel, (model, prop) => model.CoinsPrice = prop
                    );
                }

                OnPropertyChanged(nameof(ShowPrice));
                OnPropertyChanged(nameof(EXPBonus));
                OnPropertyChanged(nameof(PriceString));
                OnPropertyChanged(nameof(FullPrice));

                if (OnFullPriceChanged != null)
                {
                    OnFullPriceChanged(this, FullPrice);
                }
            }
        }

        public int FullPrice => Price * Count;

        public string PriceString => Count > 1 ? $"{Price * Count} ({Price}x{Count})" : Price.ToString();

        public string EXPBonus
        {
            get
            {
                if(InternalModel is MaterialSupport materialSupport)
                {
                    return $"– {materialSupport.ToExpEquivalent()} EXP";
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private EInventoryItemType? _type;
        public EInventoryItemType? Type
        {
            get => _type;
            set
            {
                bool initialize = _type == null;

                if (SetProperty(ref _type, value) && !initialize)
                {
                    UpdateInternalByType(value);
                }
            }
        }

        private ObservableCollection<InventoryItemVM> _payload = new();
        public ObservableCollection<InventoryItemVM> Payload
        {
            get => _payload;
            set => SetProperty(ref _payload, value);
        }

        public bool ShowPayload => Type == EInventoryItemType.Container;     
        public bool ShowCount => Count != 1 || Type == EInventoryItemType.Consumable;
        public bool ShowPrice => Type == EInventoryItemType.Equipment || Type == EInventoryItemType.Consumable;

        private RelayCommand _descriptionCommand;
        public RelayCommand DescriptionCommand
        {
            get => _descriptionCommand;
            set => SetProperty(ref _descriptionCommand, value);
        }

        private void UpdateInternalByType(EInventoryItemType? type)
        {
            if (type == null)
            {
                return;
            }

            InventoryItem newItem = InventoryItemConverter.CreateItemByType(type.Value, InternalModel);
            newItem.Id = InternalModel.Id;
            InternalModel = newItem;

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(Price));
            OnPropertyChanged(nameof(ShowCount));
            OnPropertyChanged(nameof(ShowPrice));
            OnPropertyChanged(nameof(ShowPayload));
        }

        public override string ToString() => Name;
    }

    public class InventoryItemNodeVM : InventoryItemVM
    {
        public InventoryItemNodeVM(InventoryItem model) : base(model) { }

        public ImageSource Icon { get; set; }
        public Color BackgroundColor { get; set; }
    }

    public enum EInventoryItemType
    {
        Thing,
        Container,
        Equipment,
        Consumable
    }

    public static class InventoryItemVMExtensions
    {
        public static bool IsMaterial(this EInventoryItemType type) =>
            type == EInventoryItemType.Equipment || type == EInventoryItemType.Consumable;
    }
}
