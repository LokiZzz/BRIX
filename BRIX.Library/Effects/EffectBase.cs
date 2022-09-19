using BRIX.Library.Aspects;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    public abstract class EffectBase
    {
        public List<AspectBase> Aspects;

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

        public T GetAspect<T>() where T : AspectBase
        {
            AspectBase aspect = Aspects.FirstOrDefault(x => x is T);

            if (aspect == null)
            {
                throw new ArgumentException($"У эффекта {GetType()} нет аспекта {typeof(T)}");
            }

            return (T)aspect;
        }
    }
}