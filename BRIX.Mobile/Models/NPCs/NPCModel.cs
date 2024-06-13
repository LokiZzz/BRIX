using BRIX.Library.Characters;
using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Models.Characters;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.Models.NPCs
{
    public class NPCModel : ObservableObject
    {
        public NPCModel() : this(new NPC()) { }

        public NPCModel(NPC npc)
        {
            Internal = npc;
            Abilities = new(npc.Abilities.Select(x =>
                new CharacterAbilityModel(x) { Character = npc })
            );
            Tags = new(npc.Tags.Select(x => new CharacterTagVM { Text = x }));
        }

        public NPC Internal { get; }

        private ObservableCollection<CharacterAbilityModel> _abilities = [];
        public ObservableCollection<CharacterAbilityModel> Abilities
        {
            get => _abilities;
            set => SetProperty(ref _abilities, value);
        }

        public string Name
        {
            get => Internal.Name;
            set => SetProperty(Internal.Name, value, Internal, (character, name) => character.Name = name);
        }

        public int Power => Internal.Power;

        public ObservableCollection<CharacterTagVM> Tags { get; set; }

        public void AddTag(CharacterTagVM tag)
        {
            Tags.Add(tag);
            Internal.Tags.Add(tag.Text);
        }

        public void RemoveTag(CharacterTagVM tag)
        {
            Tags.Remove(tag);
            Internal.Tags.Remove(Internal.Tags.Single(x => x == tag.Text));
        }
    }
}
