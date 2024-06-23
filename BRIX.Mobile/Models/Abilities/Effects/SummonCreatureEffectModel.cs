using BRIX.Library.Effects;
using BRIX.Mobile.Models.NPCs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public class SummonCreatureEffectModel(SummonCreatureEffect effect) 
        : EffectGenericModelBase<SummonCreatureEffect>(effect)
    {
        public SummonCreatureEffectModel() : this(new SummonCreatureEffect()) { }

        public override void Initialize(SummonCreatureEffect effect)
        {
            base.Initialize(effect);
            Creatures = new(effect.Creatures.Select(x => new SummoningCreaturesVM
            {
                Count = x.Count,
                Creature = new NPCModel(x.Creature)
            }));
        }

        public ObservableCollection<SummoningCreaturesVM> Creatures { get; set; } = [];

        public void AddCreature(NPCModel npc, int count = 1)
        {
            Internal.Creatures.Add(new CreaturesGroup { Count = count, Creature = npc.Internal });
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
            CreaturesGroup creatureToUpdate = Internal.Creatures.First(x => 
                x.Creature?.Id == newNPC.Internal.Id
            );
            int index = Internal.Creatures.IndexOf(creatureToUpdate);
            Internal.Creatures[index] = new CreaturesGroup 
            { 
                Count = creatureToUpdate.Count, 
                Creature = newNPC.Internal 
            };

            // VM collection
            SummoningCreaturesVM creatureVMToUpdate = Creatures.First(x =>
                x.Creature.Internal.Id == newNPC.Internal.Id
            );
            index = Creatures.IndexOf(creatureVMToUpdate);
            Creatures[index] = new SummoningCreaturesVM { Count = creatureVMToUpdate.Count, Creature = newNPC };
        }

        public void IncreaseCreaturesCount(SummoningCreaturesVM item)
        {
            item.Count++;
            var internalCreature = Internal.Creatures.First(x => x.Creature.Id == item.Creature.Internal.Id);
            internalCreature.Count++;
        }

        public void DecreaseCreaturesCount(SummoningCreaturesVM item)
        {
            item.Count--;
            var internalCreature = Internal.Creatures.First(x => x.Creature.Id == item.Creature.Internal.Id);
            internalCreature.Count--;
        }
    }

    public class SummoningCreaturesVM : ObservableObject
    {
        private NPCModel _creature = new();
        public NPCModel Creature
        {
            get => _creature;
            set => SetProperty(ref _creature, value);
        }

        private int _count = 1;
        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }
    }
}
