using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.DiceValue;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    public class ExhaustionEffect : EffectBase
    {
        public DicePool HealthDecrease { get; set; }

        public override List<Type> RequiredAspects => new List<Type>()
        {
            typeof(ActionPointAspect), typeof(TargetSelectionAspect),
            typeof(CooldownAspect), typeof(ActivationConditionsAspect),
            typeof(RoundDurationAspect),
        };

        public override int BaseExpCost()
        {
            int experienceEquivalent = CharacterCalculator.HealthToExp(HealthDecrease.Average());

            return (experienceEquivalent * 0.4).Round();
        }
    }
}
