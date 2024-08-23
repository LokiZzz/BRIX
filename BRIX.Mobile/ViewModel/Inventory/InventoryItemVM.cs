using BRIX.Library.Extensions;
using BRIX.Library.Items;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Inventory
{
    public partial class InventoryItemVM : ObservableObject
    {
        public InventoryItemVM(Item model)
        {
            InternalModel = model;
        }

        private Item _internalModel = new();
        public Item InternalModel
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
                    case Artifact:
                        Type = EInventoryItemType.Artifact;
                        break;
                    case Item:
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

                if (Type == EInventoryItemType.Artifact)
                {
                    OnFullPriceChanged?.Invoke(this, FullPrice);
                }
            }
        }

        public event EventHandler<int>? OnFullPriceChanged;

        public int Price
        {
            get
            {
                if(Type == EInventoryItemType.Artifact)
                {
                    return (InternalModel as Artifact)?.Price ?? 0;
                }

                return 0;
            }
        }

        public int FullPrice => Price * Count;

        public string PriceString => Count > 1 ? $"{Price * Count} ({Price}x{Count})" : Price.ToString();

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

        private ObservableCollection<InventoryItemVM> _payload = [];
        public ObservableCollection<InventoryItemVM> Payload
        {
            get => _payload;
            set => SetProperty(ref _payload, value);
        }

        public bool ShowPayload => Type == EInventoryItemType.Container;     
        public bool ShowCount => Count != 1 || Type == EInventoryItemType.Artifact;
        public bool ShowPrice => Type == EInventoryItemType.Artifact;

        private RelayCommand? _descriptionCommand;
        public RelayCommand? DescriptionCommand
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

            Item newItem = InventoryItemConverter.CreateItemByType(type.Value, InternalModel);
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

    public class InventoryItemNodeVM(Item model) : InventoryItemVM(model)
    {
        public ImageSource? Icon { get; set; }
        public Color? BackgroundColor { get; set; }
    }

    public enum EInventoryItemType
    {
        Thing,
        Container,
        Artifact
    }
}
