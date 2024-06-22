using BRIX.Library.Effects;
using BRIX.Mobile.Models.NPCs;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public class SummonCreatureEffectModel(SummonCreatureEffect effect) 
        : EffectGenericModelBase<SummonCreatureEffect>(effect)
    {
        public SummonCreatureEffectModel() : this(new SummonCreatureEffect()) 
        {
            Creatures = new(Internal.Creatures.Select(x => new SummoningCreaturesVM { 
                Count = x.Count,
                Creature = new NPCModel(x.Creature)
            }));
        }

        public ObservableCollection<SummoningCreaturesVM> Creatures { get; set; } = [];

        public void AddCreature(NPCModel npc, int count = 1)
        {
            Internal.Creatures.Add((count, npc.Internal));
            Creatures.Add(new SummoningCreaturesVM { Count = count, Creature = npc });
        }

        public void RemoveCreature(SummoningCreaturesVM creatureToRemove) 
        {
            Internal.Creatures.RemoveAll(x => x.Creature.Id == creatureToRemove.Creature.Internal.Id);
            Creatures.Remove(creatureToRemove);
        }

        public void UpdateCreature(NPCModel newNPC)
        {
            // Internal
            (int Count, Library.Characters.NPC Creature) creatureToUpdate = Internal.Creatures.First(x => 
                x.Creature.Id == newNPC.Internal.Id
            );
            int index = Internal.Creatures.IndexOf(creatureToUpdate);
            Internal.Creatures[index] = (creatureToUpdate.Count, newNPC.Internal);

            // VM collection
            SummoningCreaturesVM creatureVMToUpdate = Creatures.First(x =>
                x.Creature.Internal.Id == newNPC.Internal.Id
            );
            index = Creatures.IndexOf(creatureVMToUpdate);
            Creatures[index] = new SummoningCreaturesVM { Count = creatureVMToUpdate.Count, Creature = newNPC };
        }
    }

    public class SummoningCreaturesVM
    {
        public NPCModel Creature { get; set; } = new();

        public int Count { get; set; } = 1;
    }
}
