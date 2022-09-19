namespace BRIX.Library.Aspects
{
    public abstract class AspectBase
    {
        public virtual bool IsEnabled { get; set; }

        public abstract double GetCoefficient();

        public abstract void ConcordAspects(List<AspectBase> sameAspects);
    }
}
