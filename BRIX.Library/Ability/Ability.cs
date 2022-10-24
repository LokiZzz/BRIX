using BRIX.Library.Aspects;
using BRIX.Library.Effects;
using BRIX.Library.Extensions;

namespace BRIX.Library
{
    public class Ability
    {
        private readonly List<EffectBase> _effects = new();
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
            foreach (AspectBase aspect in effect.Aspects)
            {
                AspectBase sourceAspect = SearchSourceAspect(aspect.GetType());

                if (sourceAspect != null && (sourceAspect.IsConcording || sourceAspect is ActionPointAspect) )
                {
                    effect.Concord(sourceAspect);
                }
            }

            _effects.Add(effect);

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

        /// <summary>
        /// Получить первый эффект способности по типу.
        /// </summary>
        public T? GetEffect<T>() where T : EffectBase
        {
            return _effects.FirstOrDefault(x => x is T) as T;
        }

        public IEnumerable<T?> GetEffects<T>() where T : EffectBase
        {
            return _effects.Where(x => x is T).Select(x => x as T);
        }

        public void Clear()
        {
            _effects.Clear();
        }

        /// <summary>
        /// Удалить эффект можно только передав ссылку на него.
        /// </summary>
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