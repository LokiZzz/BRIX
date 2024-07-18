using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Ослабление. Уменьшение урона, наносимого целью.
    /// </summary>
    public class ReductionEffect : DiceImpactEffectBase
    {
        public override bool HasStatus => true;

        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect),
            typeof(DurationAspect),
        ];

        public override int BaseExpCost()
        {
            return (Impact.CostLikeDamageEffect() * 1.1).Round();
        }
    }
}
