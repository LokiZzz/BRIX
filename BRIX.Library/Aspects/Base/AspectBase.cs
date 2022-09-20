using System;

namespace BRIX.Library.Aspects
{
    public abstract class AspectBase
    {
        public virtual bool IsEnabled { get; set; }

        public abstract double GetCoefficient();

        public abstract AspectBase Concord(List<AspectBase> sameAspects);
    }
}
