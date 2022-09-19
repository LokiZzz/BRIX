using BRIX.Library.Aspects;
using BRIX.Library.DiceValue;
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
