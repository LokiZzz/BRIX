using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects.Base;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects.HealDamage
{
    public class HealOrDamageEffect : EffectBase
    {
        public HealOrDamageEffect()
        {
            Aspects = new List<AspectBase> 
            {
                new ActionPointAspect(), new TargetSelectionAspect(), new ObstacleAspect(),
                new TargetSelectionRestrictionsApsect(), new CooldownAspect(), new ActivationConditionsAspect()
            };
        }

        public bool IsDamage { get; set; } = true;

        public DicePool Impact { get; set; } = new DicePool(0);

        public override int BaseExpCost()
        {
            return Math.Pow(Impact.Average(), 2).Round();
        }
    }
}
