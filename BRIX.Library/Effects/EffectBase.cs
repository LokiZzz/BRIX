using BRIX.Library.Aspects;
using BRIX.Library.Extensions;
using System.Collections.ObjectModel;

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

        public bool TryGetAspect(Type aspectType, out AspectBase? aspect)
        {
            aspect = Aspects.FirstOrDefault(x => x.GetType() == aspectType);

            return aspect != null;
        }

        public void Concord(AspectBase sourceAspect) => SetConcordance(sourceAspect, true);

        public void Discord(AspectBase sourceAspect) => SetConcordance(sourceAspect, false);

        private void SetConcordance(AspectBase sourceAspect, bool isConcording)
        {
            if (TryGetAspect(sourceAspect.GetType(), out AspectBase aspectToConcord))
            {
                if (aspectToConcord != null)
                {
                    int index = Aspects.FindIndex(x => x.GetType().Equals(aspectToConcord.GetType()));

                    if (isConcording)
                    {
                        Aspects[index] = sourceAspect;
                    }
                    //TODO: else { Aspects[index] = UtilityHuility.GetDeepCopy(Aspects[index]); }

                    Aspects[index].IsConcording = isConcording;
                }
            }
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj?.GetType() == GetType();
        }
    }
}