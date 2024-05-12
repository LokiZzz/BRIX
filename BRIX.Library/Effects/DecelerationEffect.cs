using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.Mathematics;
using BRIX.Library.DiceValue;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Временное увеличение максимального здоровья.
    /// </summary>
    public class DecelerationEffect : DiceImpactEffectBase
    {
        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect),
            typeof(DurationAspect),
        ];

        public override int BaseExpCost()
        {
            return new ThrasholdCostConverter((1, 50), (2, 200), (3, 1000))
                .Convert(Impact.Average());
        }
    }
}
