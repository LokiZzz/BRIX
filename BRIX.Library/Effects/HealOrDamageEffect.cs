using BRIX.Library.Aspects;
using BRIX.Library.DiceValue;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    public class HealDamageEffect : EffectBase
    {
        public HealDamageEffect()
        {
            Aspects = new()
            {
                new ActionPointAspect(), new TargetSelectionAspect(), new ObstacleAspect(),
                new TargetSelectionRestrictionsApsect(), new CooldownAspect(), new ActivationConditionsAspect(),
                new TargetSizeAspect()
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
