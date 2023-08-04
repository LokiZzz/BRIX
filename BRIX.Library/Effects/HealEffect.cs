using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.DiceValue;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    public class HealEffect : EffectBase
    {
        public DicePool Impact { get; set; } = new DicePool();

        public override List<Type> RequiredAspects => new()
        {
            typeof(ActionPointAspect), typeof(TargetSelectionAspect),
            typeof(CooldownAspect), typeof(ActivationConditionsAspect)
        };

        public override int BaseExpCost()
        {
            return (Impact.Average() * Impact.Average() * 1.2).Round();
        }
    }
}
