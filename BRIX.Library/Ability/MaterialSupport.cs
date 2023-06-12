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

    public class Consumables : MaterialSupport
    {
        /// <summary>
        /// Запас расходуемого материального обеспечения выраженный в монетах
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Достаточен ли запас расходуемого материального обеспечения персонажу.
        /// Если нет — способность не может быть использована.
        /// </summary>
        public override bool IsAvailable => Stock >= CoinsPrice;

        public override double ToExpModifier => 10;

        public void Spend() => Stock -= CoinsPrice;
    }

    public static class MatirealSupportExtensions
    {
        public static double ToExpEquivalent(this MaterialSupport matSupport) 
        {
            return matSupport.CoinsPrice * matSupport.ToExpModifier;
        }
    }
}
