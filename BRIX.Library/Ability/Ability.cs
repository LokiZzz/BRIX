using BRIX.Library.Aspects;
using BRIX.Library.Effects;
using BRIX.Library.Extensions;

namespace BRIX.Library.Ability
{
    public class Ability
    {
        private readonly HashSet<EffectBase> _effects = new();
        public IReadOnlyCollection<EffectBase> Effects => _effects;

        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Постоянное материальное обеспечение.
        /// </summary>
        public AbilityMaterialSupport MaterialSupport { get; set; } = new();

        /// <summary>
        /// Расходуемое материальное обеспечение.
        /// </summary>
        public AbilityMaterialSupport Consumables { get; set; } = new();

        public double ExpCost()
        {
            double effectsCountPenaltyCoef = 1;

            if (_effects.Count() > 1)
            {
                effectsCountPenaltyCoef += (_effects.Count() - 1) * 0.2;
            }

            double expCost = _effects.Sum(effect => effect.GetExpCost()) * effectsCountPenaltyCoef;

            return (expCost - MaterialSupport.Coins / 10 - Consumables.Coins * 10).Round();
        }

        public void AddEffect(EffectBase effect)
        {
            if (!_effects.Add(effect))
            {
                throw new AbilityLogicException("Эффект такого типа уже есть в способности.");
            }

            foreach (AspectBase aspect in effect.Aspects)
            {
                AspectBase sourceAspect = SearchSourceAspect(aspect.GetType());

                if (sourceAspect != null && (sourceAspect.IsConcording || sourceAspect is ActionPointAspect) )
                {
                    effect.Concord(sourceAspect);
                }
            }

            AspectBase? SearchSourceAspect(Type aspectType)
            {
                foreach(EffectBase effect in _effects)
                {
                    if (effect.TryGetAspect(aspectType, out AspectBase? aspectToConcord))
                    {
                        return aspectToConcord;
                    }
                }

                return null;
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

        /// <summary>
        /// Дёргать этот метод после любых изменений в любом из аспектов с синхронизацией
        /// </summary>
        /// <param name="sourceAspect"></param>
        public void Concord(AspectBase sourceAspect)
        {
            if (_effects.Count() > 1)
            {
                foreach(EffectBase effect in _effects)
                {
                    effect.Concord(sourceAspect);
                }
            }
        }

        public void Discord(Type sourceAspectType)
        {
            if (_effects.Count() > 1)
            {
                foreach (EffectBase effect in _effects)
                {
                    effect.Discord(sourceAspectType);
                }
            }
        }
    }
}