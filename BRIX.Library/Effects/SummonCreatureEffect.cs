using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.Characters;

namespace BRIX.Library.Effects
{
    public class SummonCreatureEffect : EffectBase
    {
        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(DurationAspect), typeof(ActivationConditionsAspect)
        ];

        public List<(int Count, NPC Creature)> Creatures { get; set; } = [];

        public override int BaseExpCost()
        {
            return Creatures.Sum(x => x.Count * x.Creature.Power);
        }
    }
}
