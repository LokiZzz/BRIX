namespace BRIX.Mobile.ViewModel.Inventory
{
    public class InventoryItemTypeVM
    {
        public EInventoryItemType Type { get; set; }
        public string Text { get; set; } = string.Empty;

        public override string ToString() => Text;
    }
}