using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.Extensions;
using BRIX.Library.DiceValue;
using BRIX.Library.Ability;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Временное увеличение максимального здоровья.
    /// </summary>
    public class FortifyEffect : DiceImpactEffectBase
    {
        public override List<Type> RequiredAspects => new List<Type>()
        {
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect),
            typeof(DurationAspect),
        };

        public override int BaseExpCost()
        {
            int experienceEquivalent = CharacterCalculator.HealthToExp(Impact.Average());

            return (experienceEquivalent * 0.75).Round();
        }
    }
}
