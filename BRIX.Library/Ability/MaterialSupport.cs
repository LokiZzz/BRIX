using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library
{
    public abstract class MaterialSupport
    {
        public string Description { get; set; }

        public int CoinsPrice { get; set; }

        public abstract bool IsAvailable { get; }

        public abstract double ToExpModifier { get; }

        public bool IsProvided => CoinsPrice == default || IsAvailable;
    }

    public class AbilityMaterialSupport : MaterialSupport
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

    public class AbilityConsumables : MaterialSupport
    {
        /// <summary>
        /// Запас расходуемого материального обеспечения выраженный в монетах
        /// </summary>
        public int ConsumablesStock { get; set; }

        /// <summary>
        /// Достаточен ли запас расходуемого материального обеспечения персонажу.
        /// Если нет — способность не может быть использована.
        /// </summary>
        public override bool IsAvailable => ConsumablesStock >= CoinsPrice;

        public override double ToExpModifier => 10;

        public void Spend() => ConsumablesStock -= CoinsPrice;
    }

    public static class MatirealSupportExtensions
    {
        public static double ToExpEquivalent(this MaterialSupport matSupport) 
        {
            return matSupport.CoinsPrice * matSupport.ToExpModifier;
        }
    }
}
