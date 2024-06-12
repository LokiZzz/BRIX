using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Лечение.
    /// </summary>
    public class HealEffect : DiceImpactEffectBase
    {
        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect)
        ];

        public override int BaseExpCost()
        {
            return (Impact.Average() * Impact.Average() * 1.2).Round();
        }
    }
}
