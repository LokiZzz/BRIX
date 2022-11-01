using BRIX.Library.Aspects;
using BRIX.Library.Extensions;
using BRIX.Utility.Extensions;
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

        /// <summary>
        /// Устанавливает эффекту аспект по ссылке.
        /// Специально для синхронизации аспектов в разных эффектах.
        /// </summary>
        /// <param name="sourceAspect">Аспект, на который ссылаются несколько эффектов.</param>
        public void Attach(AspectBase sourceAspect)
        {
            if (TryGetAspect(sourceAspect.GetType(), out AspectBase aspectToConcord))
            {
                if (aspectToConcord != null)
                {
                    int index = Aspects.FindIndex(x => x.GetType().Equals(aspectToConcord.GetType()));
                    Aspects[index] = sourceAspect;
                    Aspects[index].IsConcording = true;
                }
            }
        }

        /// <summary>
        /// Рассинхронизирует эффект с установленным аспектом, заменяя текущую ссылку на копию.
        /// Передаваемый экземпляр не имеет значения, важен лишь его тип.
        /// </summary>
        /// <param name="sourceAspect">Любой экземпляр необходимого типа.</param>
        public void Detach(AspectBase aspectToDetach)
        {
            if (TryGetAspect(aspectToDetach.GetType(), out AspectBase aspectToConcord))
            {
                if (aspectToConcord != null)
                {
                    int index = Aspects.FindIndex(x => x.GetType().Equals(aspectToConcord.GetType()));
                    AspectBase? aspectCopy = Aspects[index].Copy();

                    if (aspectCopy != null)
                    {
                        Aspects[index] = aspectCopy;
                        Aspects[index].IsConcording = false;
                    }
                }
            }
        }
    }
}