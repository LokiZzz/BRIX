using BRIX.Library.Aspects;
using BRIX.Library.Extensions;
using BRIX.Utility.Extensions;
using System.Collections.ObjectModel;
using System.Reflection;

namespace BRIX.Library.Effects
{
    public abstract class EffectBase
    {
        /// <summary>
        /// Порядковый номер эффекта среди эффектов того же типа в способности.
        /// Идентификатор, позволяющий однозначно определять эффект в способности.
        /// </summary>
        public int Number { get; set; }

        public abstract List<Type> RequiredAspects { get; }

        public List<AspectBase> Aspects = new ();

        public abstract int BaseExpCost();

        public int GetExpCost()
        {
            double resultingCost = BaseExpCost();

            foreach (Type aspectType in RequiredAspects)
            {
                if(!typeof(AspectBase).IsAssignableFrom(aspectType))
                {
                    throw new Exception("Невалидный тип экземпляра аспекта в эффекте.");
                }

                AspectBase aspect = GetAspect(aspectType);

                if (aspect != null)
                {
                    double coeficient = aspect.GetCoefficient();
                    resultingCost = resultingCost * coeficient;
                }
                else
                {
                    throw new Exception("Ошибка полуения аспекта.");
                }
            }

            return resultingCost.Round();
        }

        public T? GetAspect<T>() where T : AspectBase
        {
            return GetAspect(typeof(T)) as T;
        }

        /// <summary>
        /// Получить аспект указанного типа. Если такого аспекта в эффекте нет, но его тип указан в списке 
        /// обязательных аспектов, то аспект будет инициализирован и возвращён в out-параметре.
        /// </summary>
        public AspectBase? GetAspect(Type aspectType)
        {
            AspectBase aspect = Aspects.FirstOrDefault(x => x.GetType() == aspectType);

            if(aspect == null)
            {
                if (RequiredAspects.Any(x => x.Equals(aspectType)))
                {
                    aspect = (AspectBase)Activator.CreateInstance(aspectType);

                    if (aspect != null)
                    {
                        Aspects.Add(aspect);
                    }
                }
                else
                {
                    throw new ArgumentException($"У эффекта {GetType()} нет аспекта {aspectType.Name}");
                }
            }

            return aspect;
        }

        /// <summary>
        /// Устанавливает эффекту аспект по ссылке.
        /// Специально для синхронизации аспектов в разных эффектах.
        /// </summary>
        /// <param name="sourceAspect">Аспект, на который ссылаются несколько эффектов.</param>
        public void Attach(AspectBase sourceAspect)
        {
            AspectBase aspectToConcord = GetAspect(sourceAspect.GetType());

            if (aspectToConcord != null)
            {
                int index = Aspects.FindIndex(x => x.GetType().Equals(aspectToConcord.GetType()));
                Aspects[index] = sourceAspect;
                Aspects[index].IsConcording = true;
            }
        }

        /// <summary>
        /// Рассинхронизирует эффект с установленным аспектом, заменяя текущую ссылку на копию.
        /// Передаваемый экземпляр не имеет значения, важен лишь его тип.
        /// </summary>
        /// <param name="sourceAspect">Любой экземпляр необходимого типа.</param>
        public void Detach(AspectBase aspectToDetach)
        {
            AspectBase searchingAspect = GetAspect(aspectToDetach.GetType());
            
            if (searchingAspect != null)
            {
                int index = Aspects.FindIndex(x => x.GetType().Equals(searchingAspect.GetType()));
                AspectBase? aspectCopy = Aspects[index].Copy();

                if (aspectCopy != null)
                {
                    Aspects[index] = aspectCopy;
                    Aspects[index].IsConcording = false;
                }
            }
        }

        public void ForceAspectInitialize()
        {
            RequiredAspects.ForEach(x => GetAspect(x));
        }
    }
}