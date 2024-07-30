using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;

namespace BRIX.Library.Effects
{
    public class InvisibiltyEffect : EffectBase
    {
        public override bool IsPositive => true;

        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(DurationAspect), typeof(ActivationConditionsAspect)
        ];

        public override int BaseExpCost() => 200;
    }
}