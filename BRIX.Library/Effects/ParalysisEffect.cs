using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.Extensions;
using BRIX.Library.Characters;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Паралич. Не даёт цели использовать способности, для активации которых персонаж должен мочь двигаться, также 
    /// цель теряет возможность перемещаться никак иначе, как способностями у которых нет соответствующего условия активации.
    /// </summary>
    public class ParalysisEffect : EffectBase
    {
        public override bool IsPositive => false;

        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect), typeof(DurationAspect), 
            typeof(TargetSizeAspect)
        ];

        /// <summary>
        /// Текущее кол-во здоровья у цели, необходимое для того, чтобы эффект на ней сработал.
        /// </summary>
        public int HealthThreshold { get; set; } = 10;

        public override int BaseExpCost() => (ExperienceCalculator.HealthToExp(HealthThreshold) * 3.5).Round();
    }
}
