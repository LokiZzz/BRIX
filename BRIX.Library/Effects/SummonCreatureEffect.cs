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

        public List<(int Count, NPC Creature)> Creatures { get; set; } = [];

        public override int BaseExpCost()
        {
            return (Creatures.Sum(x => x.Count * x.Creature.Power) * 0.85).Round();
        }
    }
}
