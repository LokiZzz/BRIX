using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Лечение.
    /// </summary>
    public class HealEffect : DamageEffect
    {
        public override bool IsPositive => true;

        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect), typeof(DurationAspect)
        ];

        public override int GetExpCost()
        {
            return (base.GetExpCost() * 1.5).Round();
        }
    }
}
