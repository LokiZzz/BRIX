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
            return (Creatures.Sum(x => x.Count * x.Creature.Power) * 0.95).Round();
        }
    }

    public class CreaturesGroup
    {
        public NPC Creature { get; set; } = new();

        public int Count { get; set; }
    }
}
