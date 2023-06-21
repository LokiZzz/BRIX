using BRIX.Library.Characters;

namespace BRIX.Mobile.ViewModel.Inventory
{
    public class InventoryItemConverter
    {
        public InventoryItemConverter()
        {
            InitializeVisual();
        }

        public InventoryItemVM ToVM(InventoryItem item)
        {
            InventoryItemVM viewModel = new(item)
            {
                Name = item.Name,
                Count = item.Count,
                BackgroundColor = isDarkBackgroundNow ? _darkItemColor : _lightItemColor,
                Description = item.Description,
            };

            switch (item)
            {
                case Container container:
                    viewModel.Type = EInventoryItemType.Container;
                    isDarkBackgroundNow = !isDarkBackgroundNow;
                    viewModel.Payload = new(container.Payload.Select(ToVM));
                    isDarkBackgroundNow = !isDarkBackgroundNow;
                    viewModel.Icon = _containerIS;
                    break;
                case Equipment equipment:
                    viewModel.Type = EInventoryItemType.Equipment;
                    viewModel.Price = equipment.CoinsPrice;
                    viewModel.Icon = _equipmentIS;
                    break;
                case Consumable consumable:
                    viewModel.Type = EInventoryItemType.Consumable;
                    viewModel.Price = consumable.CoinsPrice;
                    viewModel.Icon = _consumableIS;
                    break;
                case InventoryItem:
                    viewModel.Type = EInventoryItemType.Thing;
                    viewModel.Icon = _gemIS;
                    break;
            }

            return viewModel;
        }

        public static InventoryItem CreateItemByType(EInventoryItemType type)
        {
            switch (type)
            {
                case EInventoryItemType.Thing:
                    return new InventoryItem();
                case EInventoryItemType.Container:
                    return new Container();
                case EInventoryItemType.Equipment:
                    return new Equipment();
                case EInventoryItemType.Consumable:
                    return new Consumable();
                default: return null;
            }
        }

        private ImageSource _gemIS;
        private ImageSource _containerIS;
        private ImageSource _equipmentIS;
        private ImageSource _consumableIS;
        private bool isDarkBackgroundNow = true;
        private Color _darkItemColor;
        private Color _lightItemColor;

        private void InitializeVisual()
        {
            Application.Current.Resources.TryGetValue("BRIXMedium", out object darkItemColor);
            _darkItemColor = darkItemColor as Color;
            Application.Current.Resources.TryGetValue("BRIXDim", out object lightItemColor);
            _lightItemColor = lightItemColor as Color;
            _gemIS = ImageSource.FromFile("Inventory/gem.svg");
            _containerIS = ImageSource.FromFile("Inventory/chest.svg");
            _equipmentIS = ImageSource.FromFile("Inventory/blade.svg");
            _consumableIS = ImageSource.FromFile("Inventory/arrow.svg");
        }
    }
}
