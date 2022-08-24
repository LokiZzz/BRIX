using BRIX.Library.Extensions;

namespace BRIX.Library.Effects.Base
{
    public abstract class EffectBase
    {
        public abstract List<AspectBase> Aspects { get; }

        public abstract int BaseExpCost();

        public int GetExpCost()
        {
            double resultingCost = BaseExpCost();

            foreach (AspectBase aspect in Aspects)
            {
                double coeficient = aspect.GetCoefficient();
                resultingCost = resultingCost * coeficient;
            }

            return resultingCost.Round();
        }

        //public T GetAspect<T>() where T : AspectBase
        //{
        //    AspectBase aspect = Aspects.FirstOrDefault(x => x is T);

        //    if(aspect == null)
        //    {
        //        throw new ArgumentException($"У эффекта {GetType()} нет аспекта {typeof(T)}");
        //    }

        //    return (T)aspect;
        //}
    }
}