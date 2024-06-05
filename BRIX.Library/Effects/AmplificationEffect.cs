using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.DiceValue;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    public class AmplificationEffect : DiceImpactEffectBase
    {
        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect),
            typeof(DurationAspect),
        ];

        public override int BaseExpCost()
        {
            return (Impact.Average() * Impact.Average() * 0.8).Round();
        }
    }
}
