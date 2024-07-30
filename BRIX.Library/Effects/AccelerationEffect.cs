using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Ускорение. Временное увеличение максимального количества очков действия.
    /// </summary>
    public class AccelerationEffect : DiceImpactEffectBase
    {
        public override bool HasStatus => true;
        public override bool IsPositive => true;

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
