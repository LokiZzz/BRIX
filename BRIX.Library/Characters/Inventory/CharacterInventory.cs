namespace BRIX.Library.Characters.Inventory
{
    /// <summary>
    /// Инвентарь персонажа
    /// </summary>
    public class CharacterInventory
    {
        public int Coins { get; set; } = 100;

        /// <summary>
        /// Первый уровень инвентаря. Является вложенной структурой. Некоторые предметы могут являться контейнерами и 
        /// содержать другие предметы.
        /// </summary>
        public List<InventoryItem> Content = [];

        /// <summary>
        /// Полное перечисление всех предметов с учётом вложенности.
        /// </summary>
        public IEnumerable<InventoryItem> Items
        {
            get
            {
                foreach (InventoryItem item in Content)
                {
                    yield return item;

                    if (item is ContainerItem container)
                    {
                        foreach (InventoryItem containerItem in GoThroughRecursive(container))
                        {
                            yield return containerItem;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Рекурсивное перечисление содержимого контейнера.
        /// </summary>
        private static IEnumerable<InventoryItem> GoThroughRecursive(ContainerItem item)
        {
            foreach (InventoryItem containerItem in item.Payload)
            {
                yield return containerItem;

                if (containerItem is ContainerItem container)
                {
                    foreach (InventoryItem internalContainerItem in GoThroughRecursive(container))
                    {
                        yield return internalContainerItem;
                    }
                }
            }
        }
    }
}
