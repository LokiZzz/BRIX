using BRIX.Library.Ability;
using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.DiceValue;

namespace BRIX.Library.Effects
{
    public class DamageEffect : DiceImpactEffectBase
    {
        public string AdditionalProperty { get; set; } = "Additional property";

        public override List<Type> RequiredAspects => new() 
        {
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect)
        };

        public override int BaseExpCost()
        {
            int damageToHimselfCoef = 1;

            TargetSelectionAspect? targetSelection = GetAspect<TargetSelectionAspect>();

            if (targetSelection?.Strategy == ETargetSelectionStrategy.CharacterHimself)
            {
                damageToHimselfCoef = -1;
            }

            return Impact.Average() * Impact.Average() * damageToHimselfCoef;
        }
    }
}
