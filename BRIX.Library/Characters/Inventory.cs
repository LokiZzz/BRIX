using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Characters
{
    /// <summary>
    /// Инвентарь персонажа
    /// </summary>
    public class Inventory
    {
        public int Coins { get; set; } = 100;

        public List<InventoryItem> Content = new();

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

        private IEnumerable<InventoryItem> GoThrough(Container item)
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

        public string Name { get; set; }

        public string Description { get; set; }

        public int Count { get; set; } = 1;

        public override string ToString() =>  Name;

        public override bool Equals(object? otherObject)
        {
            InventoryItem other = otherObject as InventoryItem;

            if (other == null)
            {
                return false;
            }

            return other.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class Container : InventoryItem
    {

        public List<InventoryItem> Payload = new();
    }

    public abstract class MaterialSupport : InventoryItem
    {
        public int CoinsPrice { get; set; }

        public abstract bool IsAvailable { get; }

        public abstract double ExpModifier { get; }
    }

    public class Equipment : MaterialSupport
    {
        private bool _isAvailable;

        /// <summary>
        /// Доступно ли постоянное материальное обеспечение персонажу.
        /// Если нет — способность не может быть использована.
        /// </summary>
        public override bool IsAvailable => _isAvailable;

        public override double ExpModifier => 0.1;

        public void SetIsAvailable(bool isAvailable) => _isAvailable = isAvailable;
    }

    public class Consumable : MaterialSupport
    {
        /// <summary>
        /// Достаточен ли запас расходуемого материального обеспечения персонажу.
        /// Если нет — способность не может быть использована.
        /// </summary>
        public override bool IsAvailable => Count > 0;

        public override double ExpModifier => 10;

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
                if(item == oldItem)
                {
                    int index = inventory.Content.IndexOf(item);
                    inventory.Content[index] = newItem;

                    return;
                }

                if(item is Container container && container.Payload.Any(x => x == oldItem))
                {
                    int index = container.Payload.IndexOf(item);
                    container.Payload[index] = newItem;

                    return;
                }
            }
        }
    }
}
