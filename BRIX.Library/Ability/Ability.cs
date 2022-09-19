using BRIX.Library.Aspects;
using BRIX.Library.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Ability
{
    public class Ability
    {
        private const double EXP_INCREASE_STEP = 0.2;
        private double EffectsCountPenaltyCoef => Effects.Count > 1 ? 1 + (Effects.Count - 1) * EXP_INCREASE_STEP : 1;

        public List<EffectBase> Effects { get; } = new List<EffectBase>();
        public List<AspectBase> ConcordedAspects { get; } = new List<AspectBase>();

        public string Name { get; set; }
        public string Description { get; set; }

        public double ExpCost => EffectsCountPenaltyCoef * Effects.Sum(effect => effect.GetExpCost());

        public void Add(EffectBase effect)
        {
            Effects.Add(effect);
            NormalizeEffects(effect);
        }

        //TODO: При удалении эффектов, необходимо перезапускать нормализацию, чтобы пересчитать согласование. 
        private void NormalizeEffects(EffectBase effect)
        {
            ConcordedAspects.Clear();

            if (Effects.Count() > 1)
            {
                foreach (AspectBase aspect in effect.Aspects)
                {
                    List<AspectBase> sameAspects = GetSameAspects(aspect);

                    ConcordedAspects.Add(aspect.ConcordAspects(sameAspects));
                }
            }
        }

        private List<AspectBase> GetSameAspects<T>(T aspect) where T : AspectBase
        {
            return Effects.SelectMany(x => x.Aspects).Where(x => x is T).ToList();
        }
    }
}
