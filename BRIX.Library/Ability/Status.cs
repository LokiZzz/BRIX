using BRIX.Library.Aspects;
using BRIX.Library.Effects;
using BRIX.Library.Enums;

namespace BRIX.Library.Ability
{
    public class Status
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public int MaxDuration
        {
            get
            {
                if (Effects.Any())
                {
                    EffectBase effect = Effects.Aggregate((x, y) => 
                        x.GetAspect<DurationAspect>()?.DurationInSeconds > 
                            y.GetAspect<DurationAspect>()?.DurationInSeconds
                        ? x : y
                    );

                    return effect.GetAspect<DurationAspect>()?.Duration ?? 0;
                }
                else
                { 
                    return 0; 
                }
            }
        }

        public int DurationPassed { get; set; }

        public int DurationLeft => MaxDuration - DurationPassed >= 0 ? MaxDuration - DurationPassed : 0;

        private readonly List<EffectBase> _effects = [];
        public IReadOnlyList<EffectBase> Effects => _effects;

        public void AddEffect(EffectBase effect)
        {
            if (_effects.Contains(effect))
            {
                return;
            }

            effect.ForceAspectInitialize();

            if(!effect.Aspects.Any(x => x is DurationAspect))
            {
                throw new Exception("Статус не может содержать эффектов без аспекта длительности действия.");
            }

            effect.Id = effect.Id == Guid.Empty ? Guid.NewGuid() : effect.Id;
            _effects.Add(effect);
        }

        public void AddEffects(IEnumerable<EffectBase> effectsToAdd)
        {
            foreach(EffectBase effectToAdd in effectsToAdd)
            {
                AddEffect(effectToAdd);
            }
        }

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

        public void RemoveEffect(EffectBase? effect)
        {
            if (effect != null)
            {
                _effects.Remove(effect);
            }
        }

        public override bool Equals(object? other)
        {
            return other != null 
                && other is Status otherStatus 
                && Id == otherStatus.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Получает самую большую единицу измерения среди эффектов.
        /// </summary>
        public ETimeUnit GetHighestTimeUnit()
        {
            ETimeUnit highestTimeUnit = ETimeUnit.Round;

            foreach(EffectBase effect in Effects)
            {
                ETimeUnit? eTimeUnit = effect.GetAspect<DurationAspect>()?.Unit;

                if(eTimeUnit != null && (int)eTimeUnit > (int)highestTimeUnit)
                {
                    highestTimeUnit = eTimeUnit.Value;
                }
            }

            return highestTimeUnit;
        }
    }
}
