using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Замедление. Уменьшение максимального количества очков действия.
    /// </summary>
    public class DecelerationEffect : DiceImpactEffectBase
    {
        public override bool HasStatus => true;

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
