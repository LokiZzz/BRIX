﻿using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.DiceValue;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Временное уменьшение максимального здоровья
    /// </summary>
    public class ExhaustionEffect : SinglePropEffectBase
    {
        public override List<Type> RequiredAspects => new List<Type>()
        {
            typeof(ActionPointAspect), typeof(TargetSelectionAspect),
            typeof(CooldownAspect), typeof(ActivationConditionsAspect),
            typeof(DurationAspect),
        };

        public override int BaseExpCost()
        {
            int experienceEquivalent = CharacterCalculator.HealthToExp(Impact.Average());

            return (experienceEquivalent * 0.5).Round();
        }
    }
}
