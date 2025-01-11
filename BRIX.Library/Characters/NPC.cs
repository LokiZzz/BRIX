using BRIX.Library.Abilities;
using BRIX.Library.Effects;

namespace BRIX.Library.Characters
{
    public class NPC : CharacterBase
    {
        public int SetHealth { get; set; } = 10;

        public override int RawMaxHealth => SetHealth;

        public bool Summoned { get; set; }

        public int Power
        {
            get
            {
                int powerByAbilities = Abilities.Sum(x => x.ExpCost());
                int powerBySpeed = Speed.GetExpCost();
                int powerByHealth = ExperienceCalculator.HealthToExp(SetHealth);

                return (powerByAbilities + powerBySpeed + powerByHealth) / 2;
            }
        }

        public override bool ValidateAbility(Ability ability)
        {
            // У NPC не должно быть способностей с призывом.
            bool noSummonEffect = !ability.Effects.Any(x => x is SummonCreatureEffect);

            // У призванных существ не должно быть способностей с перезарядкой.
            bool noCooldownIfNPCWasSummoned = !Summoned || ability.Activation.HasCooldown;

            return noSummonEffect && noCooldownIfNPCWasSummoned;
        }
    }
}