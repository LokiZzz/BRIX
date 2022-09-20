using BRIX.Library.Aspects;
using BRIX.Library.Effects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Ability
{
    public class Ability
    {
        public string Name { get; set; }

        public string Description { get; set; }

        private List<EffectBase> EffectsInternal { get; } = new List<EffectBase>();
        public ReadOnlyCollection<EffectBase> Effects => EffectsInternal.AsReadOnly();

        private List<AspectBase> ConcordedAspectsInternal { get; } = new List<AspectBase>();
        public ReadOnlyCollection<AspectBase> ConcordedAspects => ConcordedAspectsInternal.AsReadOnly();

        /// <summary>
        /// Итоговая стоимость способности. Равна сумме стоимости всех эффектов, которые её составляют
        /// и увлеичивается на 20% за каждый эффект начиная со второго.
        /// </summary>
        public double ExpCost()
        {
            double effectsCountPenaltyCoef = 1;

            if (EffectsInternal.Count() > 1)
            {
                effectsCountPenaltyCoef += (EffectsInternal.Count() - 1) * 0.2;
            }

            return EffectsInternal.Sum(effect => effect.GetExpCost()) * effectsCountPenaltyCoef; 
        }

        public void Add(EffectBase effect)
        {
            if(Contains(effect))
            {
                throw new AbilityLogicException("Эффект такого типа уже есть в способности.");
            }

            EffectsInternal.Add(effect);
            Concord(effect);
        }

        public T? GetEffect<T>() where T : EffectBase
        {
            return EffectsInternal.FirstOrDefault(x => x is T) as T;
        }

        public void Clear()
        {
            EffectsInternal.Clear();
            ConcordedAspectsInternal.Clear();
        }
            
        public void Remove(EffectBase item)
        {
            EffectsInternal.Remove(item);
            Concord();
        }

        public void Remove<T>() where T : EffectBase
        {
            EffectBase? effectToRemove = EffectsInternal.FirstOrDefault(x => x is T);

            if(effectToRemove != null)
            {
                EffectsInternal.Remove(effectToRemove);
                Concord();
            }
        }

        /// <summary>
        /// Содержит ли способность данный экземпляр эффекта или эффект такого же типа.
        /// </summary>
        public bool Contains(EffectBase item)
        {
            return ContainsObject(item) || ContainsType(item);
        }

        private bool ContainsObject(EffectBase item) => EffectsInternal.Contains(item);

        private bool ContainsType(EffectBase item) => EffectsInternal.Any(x => x.GetType().Equals(item.GetType()));

        private void Concord(EffectBase initiatingEffect)
        {
            ConcordedAspectsInternal.Clear();

            if (EffectsInternal.Count() > 1)
            {
                foreach (AspectBase aspect in initiatingEffect.Aspects)
                {
                    List<AspectBase> sameAspects = GetSameAspects(aspect);
                    AspectBase concordedAspect = aspect.Concord(sameAspects);
                    ConcordedAspectsInternal.Add(concordedAspect);
                }
            }
        }

        private void Concord()
        {
            if (EffectsInternal.Any())
            {
                Concord(EffectsInternal.First());
            }
        }

        private List<AspectBase> GetSameAspects(AspectBase aspect)
        {
            return EffectsInternal.SelectMany(x => x.Aspects)
                .Where(x => x.GetType().Equals(aspect.GetType()))
                .ToList();
        }
    }
}
