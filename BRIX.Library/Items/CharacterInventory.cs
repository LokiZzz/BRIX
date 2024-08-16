namespace BRIX.Library.Items
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
        public List<Item> Content = [];

        /// <summary>
        /// Полное перечисление всех предметов с учётом вложенности.
        /// </summary>
        public IEnumerable<Item> Items
        {
            get
            {
                foreach (Item item in Content)
                {
                    yield return item;

                    if (item is ContainerItem container)
                    {
                        foreach (Item containerItem in GoThroughRecursive(container))
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
        private static IEnumerable<Item> GoThroughRecursive(ContainerItem item)
        {
            foreach (Item containerItem in item.Payload)
            {
                yield return containerItem;

                if (containerItem is ContainerItem container)
                {
                    foreach (Item internalContainerItem in GoThroughRecursive(container))
                    {
                        yield return internalContainerItem;
                    }
                }
            }
        }
    }
}
