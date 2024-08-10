using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Отмена. Отмена активации способности.
    /// </summary>
    public class CancelationEffect : EffectBase
    {
        public override bool IsPositive => false;

        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect)
        ];

        /// <summary>
        /// Максимальная мощность способности, которая может быть отменена.
        /// </summary>
        public int MaxAbilityPower { get; set; } = 100;

        public override int BaseExpCost() => MaxAbilityPower;
    }
}
