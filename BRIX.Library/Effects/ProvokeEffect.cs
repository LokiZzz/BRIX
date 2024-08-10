using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;

namespace BRIX.Library.Effects
{
    public class ProvokeEffect : EffectBase
    {
        public override bool IsPositive => false;

        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect), typeof(DurationAspect)
        ];

        public override int BaseExpCost() => 100;
    }
}
