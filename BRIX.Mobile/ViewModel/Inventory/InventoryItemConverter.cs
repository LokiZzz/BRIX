using BRIX.Library.Items;

namespace BRIX.Mobile.ViewModel.Inventory
{
    public class InventoryItemConverter
    {
        public InventoryItemConverter()
        {
            InitializeVisual();
        }

        public InventoryItemNodeVM ToVM(Item item)
        {
            InventoryItemNodeVM viewModel = new(item)
            {
                BackgroundColor = isDarkBackgroundNow ? _darkItemColor : _lightItemColor
            };

            switch (item)
            {
                case Container container:
                    isDarkBackgroundNow = !isDarkBackgroundNow;
                    viewModel.Payload = new(container.Payload.Select(ToVM));
                    isDarkBackgroundNow = !isDarkBackgroundNow;
                    viewModel.Icon = _containerIS;
                    break;
                case Item:
                    viewModel.Icon = _gemIS;
                    break;
            }

            return viewModel;
        }

        public static Item CreateItemByType(EInventoryItemType type, Item? prototype = null)
        {
            Item item = new();

            switch (type)
            {
                case EInventoryItemType.Thing:
                    item = new Item();
                    break;
                case EInventoryItemType.Container:
                    item = new Container();
                    break;
                case EInventoryItemType.Artifact:
                    item = new Artifact();
                    break;
            }

            if(prototype != null)
            {
                item.Name = prototype.Name;
                item.Description = prototype.Description;
                item.Count = prototype.Count;
            }

            return item;
        }

        private readonly ImageSource _gemIS = ImageSource.FromFile("Inventory/gem.svg");
        private readonly ImageSource _containerIS = ImageSource.FromFile("Inventory/chest.svg");

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
