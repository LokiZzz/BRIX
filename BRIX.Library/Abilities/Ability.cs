using BRIX.Library.Aspects;
using BRIX.Library.Aspects.Base;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Library.Extensions;

namespace BRIX.Library.Abilities
{
    public class Ability
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        protected readonly List<EffectBase> _effects = [];
        public IReadOnlyCollection<EffectBase> Effects => _effects;

        public readonly List<AspectBase> ConcordedAspects = [];

        public AbilityActivation Activation { get; set; } = new();

        public string StatusName { get; set; } = string.Empty;

        public bool HasStatus => Effects?.Any(x => x.HasStatus) == true;

        public virtual int ExpCost()
        {
            double effectsCountPenaltyCoef = 1;
            double deltaPerEffect = 0.2;

            // Есть эффекты, которые уменьшают стоимость способности.
            // Такие эффекты не учитываются в коэффициенте количества эффектов.
            int effectiveEffectsCount = _effects.Where(x => x.GetExpCost() > 0).Count();

            if (effectiveEffectsCount > 1)
            {
                effectsCountPenaltyCoef += (effectiveEffectsCount - 1) * deltaPerEffect;
            }
            int effectsPositiveCost = GetEffectsCost();
            // На эффекты с отрицательной стоимостью (саморазрушение) не влияют настройки активации.
            // Поэтому они считаются отдельно.
            int effectsNegativeCost = _effects.Where(x => x.GetExpCost() < 0).Sum(x => x.GetExpCost());

            double expCost = Activation.Apply(effectsPositiveCost)
                * effectsCountPenaltyCoef
                + effectsNegativeCost;

            return expCost <= 1 ? 1 : expCost.Round();
        }

        /// <summary>
        /// Расчитывает сумму стоимости эффектов с положительной стоимостью. Учитывает итоговый урон от эффектов урона,
        /// усиления и уязвимости в одной способности прогрессивно. Таким образом диверсификация урона на несколько 
        /// эффектов не становится более выгодной, чем прокачка одного эффекта.
        /// </summary>
        private int GetEffectsCost()
        {
            int cost = 0;
            DicePool overallDamageImpact = new();

            foreach (EffectBase effect in _effects.Where(x => x.GetExpCost() > 0))
            {
                //Хитрый способ, который позволяет рассчитывать стоимость наносящего урон эффекта с учётом других уже
                // добавленных подобных эффектов. Позволяет рассчитывать разные эффекты так же, как если бы вместо
               // добавления нового эффекта был просто увеличен урон.

                // Проверяем относится ли эффект к наносящим урон.
                DiceImpactEffectBase? damageImpactEffect = effect switch
                {
                    DamageEffect dmg => new DamageEffect() { Aspects = dmg.Aspects, Impact = overallDamageImpact },
                    VulnerabilityEffect vul => new VulnerabilityEffect() { Aspects = vul.Aspects, Impact = overallDamageImpact },
                    AmplificationEffect amp => new AmplificationEffect() { Aspects = amp.Aspects, Impact = overallDamageImpact },
                    ExhaustionEffect exh => new ExhaustionEffect() { Aspects = exh.Aspects, Impact = overallDamageImpact },
                    _ => null
                };

                if (damageImpactEffect != null)
                {
                    // Если относится, то вычисляем его стоимость как если бы он был частью большого общего эффекта.
                    // Вычисляем стоимость абстрактного общего эффекта до добавления очередного урона и после добавления.
                    // Разница между этими значениями — и есть относительная стоимость текущего эффекта.
                    int costBefore = damageImpactEffect.GetExpCost();
                    DiceImpactEffectBase impactEffect = (DiceImpactEffectBase)effect;
                    overallDamageImpact.Add([impactEffect.Impact]);
                    // Необходимо учесть опции броска
                    overallDamageImpact.RollOptions.CopyPropertiesFrom(impactEffect.Impact.RollOptions);
                    int costAfter = damageImpactEffect.GetExpCost();

                    cost += costAfter - costBefore;
                }
                else
                {
                    cost += effect.GetExpCost();
                }
            }

            return cost;
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
                AspectBase? existingAspect = ConcordedAspects.FirstOrDefault(
                    x => x.GetType().Equals(ConcordedAspects.GetType())
                );

                if (existingAspect != null)
                {
                    effect.Attach(existingAspect);
                }
            }

            _effects.Add(effect);
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

        public void UpdateConcordedAspect(AspectBase aspect)
        {
            int index = ConcordedAspects.IndexOf(
                ConcordedAspects.First(x => x.GetType().Equals(aspect.GetType()))
            );

            ConcordedAspects[index] = aspect;

            if (_effects.Count > 1)
            {
                foreach (EffectBase effect in _effects)
                {
                    effect.Attach(aspect);
                }
            }
        }

        /// <summary>
        /// Согласование аспекта в эффектах способности.
        /// Все эффекты будут ссылаться на один и тот же объект аспекта.
        /// </summary>
        public void Concord(AspectBase sourceAspect)
        {
            if(ConcordedAspects.Any(x => x.GetType().Equals(sourceAspect.GetType())))
            {
                return;
            }

            ConcordedAspects.Add(sourceAspect);

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
        public void Discord(Type aspectType)
        {
            ConcordedAspects.RemoveAll(x => x.GetType().Equals(aspectType));

            foreach (EffectBase effect in _effects)
            {
                effect.Detach(aspectType);
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
    }
}