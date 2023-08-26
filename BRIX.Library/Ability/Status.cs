using BRIX.Library.Aspects;
using BRIX.Library.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Ability
{
    public class Status
    {
        public string Name { get; set; }

        private List<EffectBase> _effects = new();
        public IReadOnlyList<EffectBase> Effects => _effects.AsReadOnly();

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
