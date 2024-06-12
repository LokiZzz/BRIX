using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Усиление. Увеличение урона, наносимого целью.
    /// </summary>
    public class AmplificationEffect : DiceImpactEffectBase
    {
        public override bool HasStatus => true;

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
