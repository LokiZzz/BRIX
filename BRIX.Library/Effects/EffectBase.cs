using BRIX.Library.Aspects;
using BRIX.Library.Extensions;
using BRIX.Utility.DeepCopy;
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

        public void Concord(FreeConcordanceAspect sourceAspect) => SetConcordance(sourceAspect, true);

        public void Discord(FreeConcordanceAspect sourceAspect) => SetConcordance(sourceAspect, false);

        private void SetConcordance(FreeConcordanceAspect sourceAspect, bool isConcording)
        {
            if (TryGetAspect(sourceAspect.GetType(), out AspectBase aspectToConcord))
            {
                if(aspectToConcord is FreeConcordanceAspect)
                {
                    throw new AbilityLogicException("Данный аспект не поддерживает смену режим согласования.");
                }

                if (aspectToConcord != null)
                {
                    int index = Aspects.FindIndex(x => x.GetType().Equals(aspectToConcord.GetType()));

                    Aspects[index] = isConcording
                        ? sourceAspect
                        : DeepCopyUtility.GetDeepCopy(Aspects[index]);

                    (Aspects[index] as FreeConcordanceAspect)?.SetConcordance(isConcording);
                }
            }
        }
    }
}