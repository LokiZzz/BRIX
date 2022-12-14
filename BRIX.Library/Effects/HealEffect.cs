using BRIX.Library.Aspects;
using BRIX.Library.DiceValue;
using BRIX.Library.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Effects
{
    public class HealEffect : EffectBase
    {
        private const double _healPenalty = 1.2;

        public HealEffect()
        {
            Aspects = new()
            {
                new ActionPointAspect(), new TargetSelectionAspect(), new TargetChainAspect(),
                new ObstacleAspect(), new TargetSelectionRestrictionsApsect(), new TargetSizeAspect(),
                new CooldownAspect(), new ActivationConditionsAspect(),
            };
        }

        public DicePool Impact { get; set; } = new DicePool(0);

        public override int BaseExpCost()
        {
            double expCost = Math.Pow(Impact.Average(), 2) * _healPenalty;

            return expCost.Round();
        }
    }
}
