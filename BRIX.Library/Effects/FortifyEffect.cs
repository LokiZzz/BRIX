using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Укрепление. Временное увеличение максимального здоровья.
    /// </summary>
    public class FortifyEffect : DiceImpactEffectBase
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
            // У лечения 1.2
            return Impact.CostLikeDamageEffect() * 2;
        }
    }
}
