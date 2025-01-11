using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Урон.
    /// </summary>
    public class SelfDamageEffect : DiceImpactEffectBase
    {
        public override bool IsPositive => true;

        public override List<Type> RequiredAspects => [];

        public override int BaseExpCost() => (- ExperienceCalculator.HealthToExp(Impact.Average()) / 2d).Round();
    }
}
