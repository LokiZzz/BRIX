using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.Mathematics;
using BRIX.Library.DiceValue;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Временное увеличение максимального здоровья.
    /// </summary>
    public class AccelerationEffect : DiceImpactEffectBase
    {
        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect),
            typeof(DurationAspect),
        ];

        public override int BaseExpCost()
        {
            return new ThrasholdCostConverter((1, 250), (2, 1000), (3, 5000)).Convert(Impact.Average());
        }
    }
}
