using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.Effects;
using BRIX.Library.Extensions;

namespace BRIX.Library.Abilities
{
    public class Ability
    {
        public Ability()
        {
            if (Id == Guid.Empty)
            { 
                Id = Guid.NewGuid();
            }
        }

        protected readonly List<EffectBase> _effects = [];
        public IReadOnlyCollection<EffectBase> Effects => _effects;

        private readonly HashSet<AspectBase> SynchronizingAspects = [];

        public AbilityActivation Activation { get; set; } = new();

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;

        public bool HasStatus => Effects?.Any(x => x.HasStatus) == true;

        /// <summary>
        /// Получить стоимость способности в очках опыта. 
        /// </summary>
        public int ExpCost(Func<int, int>? calculateWithDelegate = null)
        {
            double effectsCountPenaltyCoef = 1;
            double deltaPerEffect = 0.2;

            if (_effects.Count > 1)
            {
                effectsCountPenaltyCoef += (_effects.Count - 1) * deltaPerEffect;
            }

            int sumOfEffectsExpCost = _effects.Sum(effect => effect.GetExpCost());
            double activationCoef = Activation.GetCoeficient();
            int expCost = (sumOfEffectsExpCost * activationCoef * effectsCountPenaltyCoef).Round();

            if(calculateWithDelegate != null)
            {
                expCost = calculateWithDelegate(expCost);
            }

            return expCost <= 1 ? 1 : expCost;
        }

        public int ExpCost(Character? character)
        {
            int expCost = ExpCost();

            if (character != null)
            {
                IEnumerable<AbilityMaterialSupport> abilityMaterialSupport = character.MaterialSupport
                    .Where(x => x.AbilityId == Id);

                foreach (AbilityMaterialSupport item in abilityMaterialSupport)
                {
                    InventoryItem matirealSupport = character.Inventory.Items
                        .Single(x => x.Id == item.MaterialSupportId);

                    if (matirealSupport is MaterialSupport concreteItem)
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
            if(_effects.Contains(effect))
            {
                return;
            }

            effect.Id = Guid.NewGuid();
            effect.ForceAspectInitialize();

            foreach (AspectBase aspect in effect.Aspects.ToList())
            {
                AspectBase? existingAspect = SynchronizingAspects.FirstOrDefault(
                    x => x.GetType().Equals(SynchronizingAspects.GetType())
                );

                if (existingAspect != null)
                {
                    effect.Attach(existingAspect);
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
        /// Переданный эффект заменит совпавший по полю Id.
        /// </summary>
        public void UpdateEffect(EffectBase effect)
        {
            EffectBase? effectToReplace = _effects.FirstOrDefault(x => x.Id == effect.Id);

            if (effectToReplace != null)
            {
                _effects[_effects.IndexOf(effectToReplace)] = effect;
            }
            else
            {
                throw new Exception("Эффект, который треубется обновить, не найден.");
            }
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

            if (_effects.Count > 1)
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
            SynchronizingAspects.RemoveWhere(x => 
                x.GetType().Equals(sourceAspectType.GetType())
            );

            if (_effects.Count > 1)
            {
                foreach (EffectBase effect in _effects)
                {
                    effect.Detach(sourceAspectType);
                }
            }
        }

        public Status BuildStatus()
        {
            Status status = new()
            {
                Name = string.IsNullOrEmpty(StatusName) ? Name : StatusName
            };

            status.AddEffects(_effects.Where(x => x.Aspects.Any(x => x is DurationAspect)));

            return status;
        }

        public override string ToString() => Name;
    }
}