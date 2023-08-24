using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.DiceValue;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    public class HealEffect : SinglePropEffectBase
    {
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
