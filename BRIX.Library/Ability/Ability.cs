using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Characters;
using BRIX.Library.Effects;
using BRIX.Library.Extensions;

namespace BRIX.Library
{
    public class Ability
    {
        public Ability()
        {
            Guid = Guid.NewGuid();
        }

        public Ability(Guid guid)
        {
            Guid = guid;
        }

        private readonly List<EffectBase> _effects = new();
        public IReadOnlyCollection<EffectBase> Effects => _effects;

        private HashSet<AspectBase> SynchronizingAspects = new();

        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Постоянное материальное обеспечение.
        /// </summary>
        public List<Equipment> Equipment { get; set; } = new();

        /// <summary>
        /// Расходуемое материальное обеспечение.
        /// </summary>
        public List<Consumable> Consumables { get; set; } = new();

        public bool CanActivate() => Equipment.All(x => x.IsAvailable) 
            && Consumables.All(x => x.IsAvailable);

        public void Activate()
        {
            if(CanActivate())
            {
                Consumables.ForEach(x => x.Spend());
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

            int sumOfEffectsExpCost = _effects.Sum(effect => effect.GetExpCost());
            int expCost = (sumOfEffectsExpCost * effectsCountPenaltyCoef).Round();
            expCost -= Equipment.Sum(x => x.ToExpEquivalent().Round());
            expCost -= Consumables.Sum(x => x.ToExpEquivalent().Round());

            return expCost <= 1 ? 1 : expCost;
        }

        public void AddEffect(EffectBase effect)
        {
            effect.ForceAspectInitialize();
            IEnumerable<EffectBase> effectsWithSameType = _effects.Where(x => x.GetType().Equals(effect.GetType()));

            if (effectsWithSameType.Any())
            {
                effect.Number = effectsWithSameType.Count();
            }

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
                    aspect.IsConcording = true;
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

        /// <summary>
        /// Удобно для обновления эффекта в способности после его редактирования или улучшения.
        /// Переданный эффект заменит совпавший по типу и полю Number.
        /// </summary>
        public void UpdateEffect(EffectBase effectToAdd)
        {
            var effectToRemove = _effects.First(x =>
                x.Number == effectToAdd.Number && x.GetType().Equals(effectToAdd.GetType())
            );

            RemoveEffect(effectToRemove);
            AddEffect(effectToAdd);
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
        /// Согласование аспекта в эффектах способности.
        /// Все эффекты будут ссылаться на один и тот же объект аспекта.
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
        /// Отмена согласования. В каждом эффекте ссылка на общий аспект будет заменена его копией.
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