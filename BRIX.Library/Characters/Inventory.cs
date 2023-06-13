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

        public List<InventoryItem> Items = new();
    }

    public class InventoryItem
    {
        public string Name { get; set; }

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

    public static class MatirealSupportExtensions
    {
        public static double ToExpEquivalent(this MaterialSupport matSupport)
        {
            return matSupport.CoinsPrice * matSupport.ToExpModifier;
        }
    }
}
