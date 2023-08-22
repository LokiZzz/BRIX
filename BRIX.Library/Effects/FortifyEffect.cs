using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.Extensions;
using BRIX.Library.DiceValue;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Временное увеличение максимального здоровья.
    /// </summary>
    public class FortifyEffect : EffectBase
    {
        public DicePool HealthIncrease { get; set; }

        public override List<Type> RequiredAspects => new List<Type>()
        {
            typeof(ActionPointAspect), typeof(TargetSelectionAspect),
            typeof(CooldownAspect), typeof(ActivationConditionsAspect),
            typeof(RoundDurationAspect),
        };

        public override int BaseExpCost()
        {
            int experienceEquivalent = CharacterCalculator.HealthToExp(HealthIncrease.Average());

            return (experienceEquivalent * 0.75).Round();
        }
    }
}
