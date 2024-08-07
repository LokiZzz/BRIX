﻿namespace BRIX.Library.Characters
{
    /// <summary>
    /// Инвентарь персонажа
    /// </summary>
    public class Inventory
    {
        public int Coins { get; set; } = 100;

        public List<InventoryItem> Content = [];

        public IEnumerable<InventoryItem> Items
        {
            get
            { 
                foreach (InventoryItem item in Content)
                {
                    yield return item;

                    if (item is Container container)
                    {
                        foreach (InventoryItem containerItem in GoThrough(container))
                        {
                            yield return containerItem;
                        }
                    }
                }
            }
        } 

        private static IEnumerable<InventoryItem> GoThrough(Container item)
        {
            foreach (InventoryItem containerItem in item.Payload)
            {
                yield return containerItem;

                if (containerItem is Container container)
                {
                    foreach (InventoryItem internalContainerItem in GoThrough(container))
                    {
                        yield return internalContainerItem;
                    }
                }
            }
        }
    }

    public class InventoryItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Count { get; set; } = 1;

        public override string ToString() =>  Name;

        public override bool Equals(object? otherObject)
        {
            if (otherObject is not InventoryItem other)
            {
                return false;
            }

            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class Container : InventoryItem
    {

        public List<InventoryItem> Payload = [];
    }

    public abstract class MaterialSupport : InventoryItem
    {
        public int CoinsPrice { get; set; }

        public abstract bool IsAvailable { get; }

        public abstract double ExpModifier { get; }
    }

    public class Equipment : MaterialSupport
    {
        private bool _isAvailable = true;

        /// <summary>
        /// Доступно ли постоянное материальное обеспечение персонажу.
        /// Если нет — способность не может быть использована.
        /// </summary>
        public override bool IsAvailable => _isAvailable;

        /// <summary>
        /// Какую долю относительно 1 очка опыта составляет 1 монета стоимости.
        /// </summary>
        public override double ExpModifier => 0.5;

        public void SetIsAvailable(bool isAvailable) => _isAvailable = isAvailable;
    }

    public class Consumable : MaterialSupport
    {
        /// <summary>
        /// Достаточен ли запас расходуемого материального обеспечения персонажу.
        /// Если нет — способность не может быть использована.
        /// </summary>
        public override bool IsAvailable => Count > 0;

        public override double ExpModifier => 4;

        public void Spend() => Count--;
    }

    public static class InventoryExtensions
    {
        public static double ToExpEquivalent(this MaterialSupport matSupport)
        {
            return matSupport.CoinsPrice * matSupport.ExpModifier;
        }

        /// <summary>
        /// Удаление элемента из инвентаря, поиск элемента происходит по ссылке.
        /// Если вызвано удаление контейнера с сохранением его содержимого, 
        /// то содержимое сначала «вываливается» в контейнер на уровень выше, 
        /// самым последним уровнем является «корень» инвентаря.
        /// </summary>
        public static void Remove(this Inventory inventory, InventoryItem itemToDelete, bool saveContent = false)
        {
            if(saveContent && itemToDelete is Container containerToDelete)
            {
                inventory.MoveContentUpper(containerToDelete);
            }

            foreach(InventoryItem item in inventory.Items)
            {
                if(item == itemToDelete)
                {
                    inventory.Content.Remove(itemToDelete);

                    break;
                }
                else if(item is Container container && container.Payload.Contains(itemToDelete))
                {
                    container.Payload.Remove(itemToDelete);

                    break;
                }
            }
        }

        public static void MoveContentUpper(this Inventory inventory, Container containerToDelete)
        {
            if (inventory.Content.Contains(containerToDelete))
            {
                inventory.Content.AddRange(containerToDelete.Payload);
            }
            else
            {
                foreach (InventoryItem item in inventory.Items)
                {
                    if (item is Container container && container.Payload.Contains(containerToDelete))
                    {
                        container.Payload.AddRange(containerToDelete.Payload);
                    }
                }
            }
        }

        public static void Swap(this Inventory inventory, InventoryItem oldItem, InventoryItem newItem)
        {
            foreach(InventoryItem item in inventory.Items.ToList())
            {
                if(item.Equals(oldItem))
                {
                    int index = inventory.Content.IndexOf(item);
                    inventory.Content[index] = newItem;

                    return;
                }

                if(item is Container container && container.Payload.Any(x => x.Equals(oldItem)))
                {
                    int index = container.Payload.IndexOf(item);
                    container.Payload[index] = newItem;

                    return;
                }
            }
        }
    }
}
