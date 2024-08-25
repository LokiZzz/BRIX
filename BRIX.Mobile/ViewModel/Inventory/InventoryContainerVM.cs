using BRIX.Library.Items;

namespace BRIX.Mobile.ViewModel.Inventory
{
    public class InventoryContainerVM
    {
        public Container? OriginalModelRefernece { get; set; }
        public string Name { get; set; } = string.Empty;

        public override string ToString() => Name;
    }
}