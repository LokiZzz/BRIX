using BRIX.Library.Characters.Inventory;

namespace BRIX.Mobile.ViewModel.Inventory
{
    public class InventoryItemConverter
    {
        public InventoryItemConverter()
        {
            InitializeVisual();
        }

        public InventoryItemNodeVM ToVM(InventoryItem item)
        {
            InventoryItemNodeVM viewModel = new(item)
            {
                BackgroundColor = isDarkBackgroundNow ? _darkItemColor : _lightItemColor
            };

            switch (item)
            {
                case ContainerItem container:
                    isDarkBackgroundNow = !isDarkBackgroundNow;
                    viewModel.Payload = new(container.Payload.Select(ToVM));
                    isDarkBackgroundNow = !isDarkBackgroundNow;
                    viewModel.Icon = _containerIS;
                    break;
                case ConsumableItem consumable:
                    viewModel.Icon = _consumableIS;
                    break;
                case InventoryItem:
                    viewModel.Icon = _gemIS;
                    break;
            }

            return viewModel;
        }

        public static InventoryItem CreateItemByType(EInventoryItemType type, InventoryItem? prototype = null)
        {
            InventoryItem item = new();

            switch (type)
            {
                case EInventoryItemType.Thing:
                    item = new InventoryItem();
                    break;
                case EInventoryItemType.Container:
                    item = new ContainerItem();
                    break;
                case EInventoryItemType.Consumable:
                    item = new ConsumableItem();
                    break;
            }

            if(prototype != null)
            {
                item.Name = prototype.Name;
                item.Description = prototype.Description;
                item.Count = prototype.Count;

                if(item is ConsumableItem matirealSupport && prototype is ConsumableItem matirealSupportPrototype)
                {
                    matirealSupport.Price = matirealSupportPrototype.Price;
                }
            }

            return item;
        }

        private readonly ImageSource _gemIS = ImageSource.FromFile("Inventory/gem.svg");
        private readonly ImageSource _containerIS = ImageSource.FromFile("Inventory/chest.svg");
        private readonly ImageSource _consumableIS = ImageSource.FromFile("Inventory/arrow.svg");

        private bool isDarkBackgroundNow = true;

        private Color? _darkItemColor;
        private Color? _lightItemColor;

        private void InitializeVisual()
        {
            object? darkItemColor = null;
            Application.Current?.Resources.TryGetValue("BRIXMedium", out darkItemColor);
            _darkItemColor = darkItemColor as Color;

            object? lightItemColor = null;
            Application.Current?.Resources.TryGetValue("BRIXDim", out lightItemColor);
            _lightItemColor = lightItemColor as Color;
        }
    }
}
