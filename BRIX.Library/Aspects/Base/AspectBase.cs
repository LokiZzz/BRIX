namespace BRIX.Library.Aspects
{
    public abstract class AspectBase
    {
        /// <summary>
        /// Показывает, синхронизируется ли аспект с другими аспектами того же типа в других эффектах способности.
        /// </summary>
        public bool IsConcording { get; set; }

        public abstract double GetCoefficient();
    }
}
