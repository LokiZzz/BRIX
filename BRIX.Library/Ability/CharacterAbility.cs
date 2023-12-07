using BRIX.Library.Ability;
using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.Effects;
using BRIX.Library.Extensions;

namespace BRIX.Library
{
    public class CharacterAbility
    {
        public CharacterAbility()
        {
            Id = Guid.NewGuid();
        }

        public CharacterAbility(Guid guid)
        {
            Id = guid;
        }

        private readonly List<EffectBase> _effects = new();
        public IReadOnlyCollection<EffectBase> Effects => _effects;

        private HashSet<AspectBase> SynchronizingAspects = new();

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;

        public bool HasStatus => Effects?.Any(x => x.Aspects.Any(x => x is DurationAspect)) == true;

        /// <summary>
        /// Получить стоимость способности в очках опыта. 
        /// Можно передать персонажа, для которого будет рассчитана индивидуальная стоимость.
        /// Индивидуальная стоимость может происходить из того, что у персонажа будет материальное обеспечение для
        /// данной способности.
        /// </summary>
        public int ExpCost(Character? character = null)
        {
            double effectsCountPenaltyCoef = 1;
            double deltaPerEffect = 0.2;

            if (_effects.Count() > 1)
            {
                effectsCountPenaltyCoef += (_effects.Count() - 1) * deltaPerEffect;
            }

            int sumOfEffectsExpCost = _effects.Sum(effect => effect.GetExpCost());
            int expCost = (sumOfEffectsExpCost * effectsCountPenaltyCoef).Round();

            if (character != null)
            {
                IEnumerable<AbilityMaterialSupport> abilityMaterialSupport = character.MaterialSupport
                    .Where(x => x.AbilityId == Id);

                foreach (AbilityMaterialSupport item in abilityMaterialSupport)
                {
                    MaterialSupport? concreteItem = character.Inventory.Items
                        .Single(x => x.Id == item.MaterialSupportId) as MaterialSupport;

                    if (concreteItem != null)
                    {
                        expCost -= concreteItem.ToExpEquivalent().Round();
                    }
                }
            }

            return expCost <= 1 ? 1 : expCost;
        }

        public T? GetAspect<T>() where T : AspectBase
        {
            AspectBase? aspect = SynchronizingAspects.FirstOrDefault(x => x.GetType().Equals(typeof(T)));

            return aspect as T;
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
                AspectBase? existingAspect = SynchronizingAspects.FirstOrDefault(
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
        public void UpdateEffect(EffectBase effect)
        {
            EffectBase effectToRemove = _effects.First(x =>
                x.Number == effect.Number && x.GetType().Equals(effect.GetType())
            );

            RemoveEffect(effectToRemove);
            AddEffect(effect);
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

        public Status BuildStatus()
        {
            Status status = new Status();

            status.Name = string.IsNullOrEmpty(StatusName) ? Name : StatusName;
            status.AddEffects(_effects.Where(x => x.Aspects.Any(x => x is DurationAspect)));

            return status;
        }

        public override string ToString() => Name;
    }
}