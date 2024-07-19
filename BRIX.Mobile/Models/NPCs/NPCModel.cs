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
            Speed = new CharacterSpeedModel(npc.Speed, () => OnPropertyChanged(nameof(Power)));
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

        public string Description
        {
            get => Internal.Description;
            set => SetProperty(Internal.Description, value, Internal, (character, desc) => character.Description = desc);
        }

        public int Health
        {
            get => Internal.Health;
            set
            {
                SetProperty(Internal.Health, value, Internal, (model, prop) => model.Health = prop);
                OnPropertyChanged(nameof(Power));
            }
        }

        public CharacterSpeedModel Speed { get; set; }

        public int Power => Internal.Power;

        public void AddAbility(CharacterAbilityModel ability)
        {
            Internal.Abilities.Add(ability.Internal);
            Abilities.Add(ability);
            OnPropertyChanged(nameof(Power));
        }

        public void RemoveAbility(Guid abilityGuid)
        {
            Internal.Abilities.RemoveAll(x => x.Id == abilityGuid);
            Abilities.Remove(Abilities.First(x => x.Internal.Id == abilityGuid));
            OnPropertyChanged(nameof(Power));
        }

        public void UpdateAbility(CharacterAbilityModel ability)
        {
            int indexOfOldAbility = Abilities.IndexOf(
                Abilities.First(x => x.Internal.Id == ability.Internal.Id)
            );
            Abilities[indexOfOldAbility] = ability;

            indexOfOldAbility = Internal.Abilities.IndexOf(
                Internal.Abilities.First(x => x.Id == ability.Internal.Id)
            );
            Internal.Abilities[indexOfOldAbility] = ability.Internal;
            OnPropertyChanged(nameof(Power));
        }

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
