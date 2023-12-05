using BRIX.Library.Characters;

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
            InventoryItemNodeVM viewModel = new(item);
            viewModel.BackgroundColor = isDarkBackgroundNow ? _darkItemColor : _lightItemColor;

            switch (item)
            {
                case Container container:
                    isDarkBackgroundNow = !isDarkBackgroundNow;
                    viewModel.Payload = new(container.Payload.Select(ToVM));
                    isDarkBackgroundNow = !isDarkBackgroundNow;
                    viewModel.Icon = _containerIS;
                    break;
                case Equipment equipment:
                    viewModel.Icon = _equipmentIS;
                    break;
                case Consumable consumable:
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
            InventoryItem item = new InventoryItem();

            switch (type)
            {
                case EInventoryItemType.Thing:
                    item = new InventoryItem();
                    break;
                case EInventoryItemType.Container:
                    item = new Container();
                    break;
                case EInventoryItemType.Equipment:
                    item = new Equipment();
                    break;
                case EInventoryItemType.Consumable:
                    item = new Consumable();
                    break;
            }

            if(prototype != null)
            {
                item.Name = prototype.Name;
                item.Description = prototype.Description;
                item.Count = prototype.Count;

                if(item is MaterialSupport matirealSupport && prototype is MaterialSupport matirealSupportPrototype)
                {
                    matirealSupport.CoinsPrice = matirealSupportPrototype.Count;
                }
            }

            return item;
        }

        private ImageSource _gemIS = ImageSource.FromFile("Inventory/gem.svg");
        private ImageSource _containerIS = ImageSource.FromFile("Inventory/chest.svg");
        private ImageSource _equipmentIS = ImageSource.FromFile("Inventory/blade.svg");
        private ImageSource _consumableIS = ImageSource.FromFile("Inventory/arrow.svg");
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
