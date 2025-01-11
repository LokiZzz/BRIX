using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.Extensions;
using BRIX.Library.Characters;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Немота. Не даёт цели использовать способности, для активации которых персонаж должен мочь говорить.
    /// </summary>
    public class MutenessEffect : EffectBase
    {
        public override bool IsPositive => false;

        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect), typeof(DurationAspect)
        ];

        /// <summary>
        /// Текущее кол-во здоровья у цели, необходимое для того, чтобы эффект на ней сработал.
        /// </summary>
        public int HealthThreshold { get; set; } = 10;

        public override int BaseExpCost() => ExperienceCalculator.HealthToExp(HealthThreshold) / 2;
    }
}
