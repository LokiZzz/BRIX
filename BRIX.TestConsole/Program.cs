// See https://aka.ms/new-console-template for more information
using BRIX.Library;
using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Library.Mathematics;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.Intrinsics.Arm;
using System.Text.Json;

Character character = new ();
Ability ability = new ();
ability.AddEffect(new DamageEffect() { Impact = new DicePool((3, 6)) });
character.Abilities.Add(ability);

JsonSerializerSettings settings = new ()
{
    Formatting = Formatting.Indented,
    TypeNameHandling = TypeNameHandling.All,
};

string json = JsonConvert.SerializeObject(character, settings);

Character deserialized = JsonConvert.DeserializeObject<Character>(json, settings);

Console.ReadLine();