using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    public class SummonCreatureEffect : EffectBase
    {
        public override List<Type> RequiredAspects =>
        [
            typeof(AOEAspect), typeof(DurationAspect), typeof(ActivationConditionsAspect)
        ];

        public List<CreaturesGroup> Creatures { get; set; } = [];

        public override int BaseExpCost()
        {
            return Creatures.Sum(x => x.Count * Math.Min(x.Count, 3) * GetPowerForSummon(x.Creature));
        }

        private int GetPowerForSummon(NPC creature)
        {
            double initialPower = creature.Power;
            initialPower *= creature.Abilities.Any(x => x.Effects.Any(y => y is VulnerabilityEffect)) ? 3.5 : 1;
            
            return Math.Max(initialPower.Round(), 50);
        }
    }

    public class CreaturesGroup
    {
        public NPC Creature { get; set; } = new();

        public int Count { get; set; }
    }
}
