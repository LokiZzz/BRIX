namespace BRIX.Library.Items
{
    public static class InventoryExtensions
    {
        /// <summary>
        /// Удаление элемента из инвентаря, поиск элемента происходит по ссылке.
        /// Если вызвано удаление контейнера с сохранением его содержимого, 
        /// то содержимое сначала «вываливается» в контейнер на уровень выше, 
        /// самым последним уровнем является «корень» инвентаря.
        /// </summary>
        public static void Remove(this CharacterInventory inventory, Item itemToDelete, bool saveContent = false)
        {
            if (saveContent && itemToDelete is ContainerItem containerToDelete)
            {
                inventory.MoveContentUpper(containerToDelete);
            }

            foreach (Item item in inventory.Items)
            {
                if (item == itemToDelete)
                {
                    inventory.Content.Remove(itemToDelete);

                    break;
                }
                else if (item is ContainerItem container && container.Payload.Contains(itemToDelete))
                {
                    container.Payload.Remove(itemToDelete);

                    break;
                }
            }
        }

        public static void MoveContentUpper(this CharacterInventory inventory, ContainerItem containerToDelete)
        {
            if (inventory.Content.Contains(containerToDelete))
            {
                inventory.Content.AddRange(containerToDelete.Payload);
            }
            else
            {
                foreach (Item item in inventory.Items)
                {
                    if (item is ContainerItem container && container.Payload.Contains(containerToDelete))
                    {
                        container.Payload.AddRange(containerToDelete.Payload);
                    }
                }
            }
        }

        public static void Swap(this CharacterInventory inventory, Item oldItem, Item newItem)
        {
            foreach (Item item in inventory.Items.ToList())
            {
                if (item.Equals(oldItem))
                {
                    int index = inventory.Content.IndexOf(item);
                    inventory.Content[index] = newItem;

                    return;
                }

                if (item is ContainerItem container && container.Payload.Any(x => x.Equals(oldItem)))
                {
                    int index = container.Payload.IndexOf(item);
                    container.Payload[index] = newItem;

                    return;
                }
            }
        }
    }
}
