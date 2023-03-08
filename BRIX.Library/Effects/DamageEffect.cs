using BRIX.Library.Aspects;
using BRIX.Library.DiceValue;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    public class DamageEffect : EffectBase
    {
        public DamageEffect()
        {
            Aspects = new()
            {
                new ActionPointAspect(), new TargetSelectionAspect(), new TargetChainAspect(),
                new ObstacleAspect(), new TargetSelectionRestrictionsApsect(), new CooldownAspect(), 
                new ActivationConditionsAspect(),
            };
        }

        public DicePool Impact { get; set; } = new DicePool(0);

        public override int BaseExpCost() => Impact.Average() * Impact.Average();
    }
}
