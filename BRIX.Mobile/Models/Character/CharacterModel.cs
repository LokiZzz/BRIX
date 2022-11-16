using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CharacterBM = BRIX.Library.Character.Character;

namespace BRIX.Mobile.Models.Character
{
    public class CharacterModel : ObservableObject
    {
        public CharacterModel() : this(new CharacterBM()) { }

        public CharacterModel(CharacterBM character) => Character = character;

        public CharacterBM Character { get; }

        public Guid Id
        {
            get => Character.Id;
            set => SetProperty(Character.Id, value, Character, (character, id) => character.Id = id);
        }

        public string Name
        {
            get => Character.Name;
            set => SetProperty(Character.Name, value, Character, (character, name) => character.Name = name);
        }

        public string Backstory
        {
            get => Character.Backstory;
            set => SetProperty(Character.Backstory, value, Character, (character, backstory) => character.Backstory = backstory);
        }

        public string Appearance
        {
            get => Character.Appearance;
            set => SetProperty(Character.Appearance, value, Character, (character, appearance) => character.Appearance = appearance);
        }
    }
}
