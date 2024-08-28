namespace BRIX.Library.Aspects.Base
{
    public abstract class AspectBase
    {
        /// <summary>
        /// Показывает, синхронизируется ли аспект с другими аспектами того же типа в других эффектах способности.
        /// </summary>
        public bool IsConcorded { get; set; }

        public abstract double GetCoefficient();

        public virtual void Initialize() { }
    }
}
