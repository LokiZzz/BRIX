using BRIX.Library.Aspects;
using BRIX.Library.DiceValue;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    public class HealDamageEffect : EffectBase
    {
        private const double _healPenalty = 1.2;

        public HealDamageEffect()
        {
            Aspects = new()
            {
                new ActionPointAspect(), new TargetSelectionAspect(), new TargetChainAspect(),
                new ObstacleAspect(), new TargetSelectionRestrictionsApsect(), new TargetSizeAspect(), 
                new CooldownAspect(), new ActivationConditionsAspect(),
            };
        }

        public DicePool Impact { get; set; } = new DicePool(0);

        public EHPImpact ImpactType = EHPImpact.Damage;

        public override int BaseExpCost()
        {
            double modifier = ImpactType == EHPImpact.Heal ? _healPenalty : 1;

            return (Impact.Average() * Impact.Average() * modifier).Round();
        }
    }

    public enum EHPImpact
    {
        Damage = 0,
        Heal = 1
    }
}
