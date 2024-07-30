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
        public override bool IsPositive => true;

        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect)
        ];

        public override int BaseExpCost()
        {
            //Лечиться должно быть менее выгодно для улучшения динамики в сражениях.
            return (Impact.CostLikeDamageEffect() * 1.2).Round();
        }
    }
}
