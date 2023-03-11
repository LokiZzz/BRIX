// See https://aka.ms/new-console-template for more information
using BRIX.Library;
using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Library.Mathematics;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Text.Json;

Character character = new ();
Ability ability = new ();
DamageEffect damage = new DamageEffect() { Impact = new DicePool((3, 6)) };
ActionPointAspect apAspect = damage.GetAspect<ActionPointAspect>();
if (apAspect != null)
{
    apAspect.ActionPoints = 4;
}
ability.AddEffect(damage);
character.Abilities.Add(ability);

JsonSerializerSettings settings = new ()
{
    Formatting = Formatting.Indented,
    TypeNameHandling = TypeNameHandling.All,
};

string json = JsonConvert.SerializeObject(character, settings);

Character deserialized = JsonConvert.DeserializeObject<Character>(json, settings);

Console.ReadLine();