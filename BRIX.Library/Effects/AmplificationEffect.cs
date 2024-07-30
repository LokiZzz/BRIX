using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Усиление. Увеличение урона, наносимого целью.
    /// </summary>
    public class AmplificationEffect : DiceImpactEffectBase
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
            // Коэффициент эффективности. Если услить только одного персонажа, у которого способность наносит урон 
            // сразу нескольким противникам, то мощь партии начинает неконтролируемо расти, поэтому усиление должно
            // быть дороже, чем сам урон.
            int effectivenessCoef = 2;

            return Impact.CostLikeDamageEffect() * effectivenessCoef;
        }
    }
}
