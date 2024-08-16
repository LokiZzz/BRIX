namespace BRIX.Library.Characters.Inventory
{
    public class ContainerItem : InventoryItem
    {
        /// <summary>
        /// Содержимое контейнера.
        /// </summary>
        public List<InventoryItem> Payload = [];
    }
}
