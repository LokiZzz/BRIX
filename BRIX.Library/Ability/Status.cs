using BRIX.Library.Aspects;
using BRIX.Library.Effects;

namespace BRIX.Library.Ability
{
    public class Status
    {
        public string Name { get; set; }

        public int RoundsPassed { get; set; }

        public int MaxDuration
        {
            get
            {
                if (Effects.Any())
                {
                    return Effects.Max(x => x.GetAspect<RoundDurationAspect>()?.Rounds ?? 0);
                }
                else
                { 
                    return 0; 
                }
            }
        }
        public int RoundsLeft => MaxDuration - RoundsPassed >= 0 ? MaxDuration - RoundsPassed : 0;

        private readonly List<EffectBase> _effects = new();
        public IReadOnlyList<EffectBase> Effects => _effects;

        public void AddEffect(EffectBase effect)
        {
            effect.ForceAspectInitialize();

            if(!effect.Aspects.Any(x => x is RoundDurationAspect))
            {
                throw new Exception("Статус не может содержать эффектов без аспекта длительности действия.");
            }

            _effects.Add(effect);
        }

        public void RemoveEffect(EffectBase? effect)
        {
            if (effect != null)
            {
                _effects.Remove(effect);
            }
        }
    }
}
