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
        public int Coins { get; set; }

        public List<InventoryItem> Content = new();

        public IEnumerable<InventoryItem> Items()
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
        public string Name { get; set; }

        public string Description { get; set; }

        public int Count { get; set; } = 1;

        public int WeightKg { get; set; }
    }

    public class Container : InventoryItem
    {
        public List<InventoryItem> Payload = new();
    }

    public abstract class MaterialSupport : InventoryItem
    {
        public int CoinsPrice { get; set; }

        public abstract bool IsAvailable { get; }

        public abstract double ToExpModifier { get; }
    }

    public class Equipment : MaterialSupport
    {
        private bool _isAvailable;

        /// <summary>
        /// Доступно ли постоянное материальное обеспечение персонажу.
        /// Если нет — способность не может быть использована.
        /// </summary>
        public override bool IsAvailable => _isAvailable;

        public override double ToExpModifier => 0.1;

        public void SetIsAvailable(bool isAvailable) => _isAvailable = isAvailable;
    }

    public class Consumable : MaterialSupport
    {
        /// <summary>
        /// Достаточен ли запас расходуемого материального обеспечения персонажу.
        /// Если нет — способность не может быть использована.
        /// </summary>
        public override bool IsAvailable => Count > 0;

        public override double ToExpModifier => 10;

        public void Spend() => Count--;
    }

    public static class InventoryExtensions
    {
        public static double ToExpEquivalent(this MaterialSupport matSupport)
        {
            return matSupport.CoinsPrice * matSupport.ToExpModifier;
        }

        //public static void Remove(this Inventory inventory, )
        //{
        //    return matSupport.CoinsPrice * matSupport.ToExpModifier;
        //}
    }
}
