using BRIX.Library.Aspects;
using BRIX.Library.Effects;
using BRIX.Library.Extensions;

namespace BRIX.Library
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
        public AbilityConsumables Consumables { get; set; } = new();

        public bool CanActivate() => MaterialSupport.IsProvided && Consumables.IsProvided;

        public void Activate()
        {
            if(CanActivate())
            {
                Consumables.Spend();
            }
        }

        public int ExpCost()
        {
            double effectsCountPenaltyCoef = 1;

            if (_effects.Count() > 1)
            {
                effectsCountPenaltyCoef += (_effects.Count() - 1) * 0.2;
            }

            double expCost = _effects.Sum(effect => effect.GetExpCost()) * effectsCountPenaltyCoef;

            return (expCost - MaterialSupport.CoinsPrice / 10 - Consumables.CoinsPrice * 10).Round();
        }

        public void AddEffect(EffectBase effect)
        {
            foreach (AspectBase aspect in effect.Aspects)
            {
                AspectBase sourceAspect = SearchSourceAspect(aspect.GetType());

                if (sourceAspect != null && (sourceAspect.IsConcording || sourceAspect is ActionPointAspect) )
                {
                    effect.Concord(sourceAspect);
                }
            }

            if (!_effects.Add(effect))
            {
                throw new AbilityLogicException("Эффект такого типа уже есть в способности.");
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
        /// Включение синхронизации аспекта в эффектах и синхронизация данных аспекта.
        /// Все аспекты будут синхронизированы с переданным.
        /// </summary>
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

        /// <summary>
        /// Отключение синхронизации аспекта в эффектах
        /// </summary>
        public void Discord(AspectBase sourceAspectType)
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