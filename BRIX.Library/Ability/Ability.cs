using BRIX.Library.Aspects;
using BRIX.Library.Effects;
using BRIX.Library.Extensions;
using BRIX.Utility.Extensions;

namespace BRIX.Library
{
    public class Ability
    {
        private readonly List<EffectBase> _effects = new();
        public IReadOnlyCollection<EffectBase> Effects => _effects;

        private HashSet<AspectBase> SynchronizingAspects = new();

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
            double deltaPerEffect = 0.2;

            if (_effects.Count() > 1)
            {
                effectsCountPenaltyCoef += (_effects.Count() - 1) * deltaPerEffect;
            }

            double expCost = _effects.Sum(effect => effect.GetExpCost()) * effectsCountPenaltyCoef;

            return (expCost - MaterialSupport.ToExpEquivalent() - Consumables.ToExpEquivalent()).Round();
        }

        public void AddEffect(EffectBase effect)
        {
            foreach (AspectBase aspect in effect.Aspects.ToList())
            {
                AspectBase existingAspect = SynchronizingAspects.FirstOrDefault(
                    x => x.GetType().Equals(SynchronizingAspects.GetType())
                );

                if (existingAspect != null)
                {
                    effect.Attach(existingAspect);
                }
                else if(aspect is ActionPointAspect)
                {
                    SynchronizingAspects.Add(aspect);
                }
            }

            _effects.Add(effect);
        }

        public IEnumerable<T?> GetEffects<T>() where T : EffectBase
        {
            return _effects.Where(x => x is T).Select(x => x as T);
        }

        public void Clear()
        {
            _effects.Clear();
        }

        public void RemoveEffect(EffectBase item)
        {
            _effects.Remove(item);
        }

        public bool Contains(EffectBase item)
        {
            return _effects.Contains(item);
        }

        /// <summary>
        /// Включение синхронизации аспекта в эффектах и синхронизация данных аспекта.
        /// Все аспекты будут синхронизированы с переданным, то есть ссылаться на один и тот же объект.
        /// </summary>
        public void Concord(AspectBase sourceAspect)
        {
            SynchronizingAspects.Add(sourceAspect);

            if (_effects.Count() > 1)
            {
                foreach(EffectBase effect in _effects)
                {
                    effect.Attach(sourceAspect);
                }
            }
        }

        /// <summary>
        /// Отключение синхронизации аспекта во всех эффектах и «отключение» экземпляра аспекта от способности.
        /// </summary>
        public void Discord(AspectBase sourceAspectType)
        {
            if (sourceAspectType is not ActionPointAspect)
            {
                SynchronizingAspects.RemoveWhere(x => 
                    x.GetType().Equals(sourceAspectType.GetType())
                );

                if (_effects.Count() > 1)
                {
                    foreach (EffectBase effect in _effects)
                    {
                        effect.Detach(sourceAspectType);
                    }
                }
            }
            else
            {
                throw new AbilityLogicException("Нельзя рассинхронизировать аспект очков действий.");
            }
        }
    }
}