﻿using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Истощение. Временное уменьшение максимального здоровья
    /// </summary>
    public class ExhaustionEffect : DiceImpactEffectBase
    {
        public override bool IsPositive => false;
        
        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect),
            typeof(DurationAspect),
        ];

        public override int BaseExpCost()
        {
            return (Impact.CostLikeDamageEffect() * 1.75).Round();
        }
    }
}
