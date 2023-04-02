using BRIX.Library.Aspects;
using BRIX.Library.DiceValue;

namespace BRIX.Library.Effects
{
    public class DamageEffect : EffectBase
    {
        public DicePool Impact { get; set; } = new DicePool(0);

        public override List<Type> RequiredAspects => new() 
        {
                typeof(ActionPointAspect), typeof(TargetSelectionAspect), typeof(TargetChainAspect),
                typeof(ObstacleAspect), typeof(TargetSelectionRestrictionsApsect), typeof(CooldownAspect),
                typeof(ActivationConditionsAspect)
        };

        public override int BaseExpCost() => Impact.Average() * Impact.Average();
    }
}
