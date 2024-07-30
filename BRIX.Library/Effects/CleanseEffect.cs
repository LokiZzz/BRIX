using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Очищение. Снимает статусы с цели.
    /// </summary>
    public class CleanseEffect : EffectBase
    {
        public override bool IsPositive => true;

        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect)
        ];

        /// <summary>
        /// Максимальная мощность статуса, который будет снят с цели.
        /// </summary>
        public int MaxStatusPower { get; set; } = 100;

        /// <summary>
        /// Максимальное количество статусов которые могут быть сняты с цели.
        /// </summary>
        public int StatusesCount { get; set; } = 1;

        public override int BaseExpCost() => (MaxStatusPower * StatusesCount * 0.8).Round();
    }
}
