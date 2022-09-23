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
        private readonly HashSet<EffectBase> _effects = new();

        public string Name { get; set; }
        public string Description { get; set; }
        public IReadOnlyCollection<EffectBase> Effects => _effects;
        //В методе конкорд необходимо убедиться, что в коллекции лежит
        //новый экземпляр аспекта, а не ссылка на один из аспектов одного из эффектов.
        public IReadOnlyCollection<AspectBase> ConcordedAspects => Concord(_effects);

        public double ExpCost()
        {
            double effectsCountPenaltyCoef = 1;

            if (_effects.Count() > 1)
            {
                effectsCountPenaltyCoef += (_effects.Count() - 1) * 0.2;
            }

            return _effects.Sum(effect => effect.GetExpCost()) * effectsCountPenaltyCoef;
        }

        public void AddEffect(EffectBase effect)
        {
            if (!_effects.Add(effect))
            {
                throw new AbilityLogicException("Эффект такого типа уже есть в способности.");
            }
        }

        public T? GetEffect<T>() where T : EffectBase
        {
            return _effects.FirstOrDefault(x => x is T) as T;
        }

        public void Clear()
        {
            _effects.Clear();
        }

        public void RemoveEffect(EffectBase item)
        {
            _effects.Remove(item);
        }

        public void RemoveEffect<T>() where T : EffectBase
        {
            _effects.RemoveWhere(x => x is T);
        }

        public bool Contains(EffectBase item)
        {
            return _effects.Contains(item);
        }

        private List<AspectBase> Concord(IEnumerable<EffectBase> effects)
        {
            List<AspectBase> aspects = new();

            if (effects.Count() > 1)
            {
                foreach (AspectBase aspect in effects.First().Aspects)
                {
                    List<AspectBase> sameAspects = GetSameAspects(effects, aspect);
                    AspectBase concordedAspect = aspect.Concord(sameAspects);
                    aspects.Add(concordedAspect);
                }
            }
            return aspects;
        }

        private List<AspectBase> GetSameAspects(IEnumerable<EffectBase> effects, AspectBase aspect)
        {
            return effects.SelectMany(x => x.Aspects)
                .Where(x => x.GetType().Equals(aspect.GetType()))
                .ToList();
        }
    }
}
