using BRIX.Lexica;
using BRIX.Library.Abilities;
using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Library.Mathematics;
using BRIX.Utility.Extensions;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

Ability ability1 = new();

ability1.AddEffect(new DamageEffect { Impact = new((1, 4)) });
ability1.AddEffect(new DamageEffect { Impact = new((1, 4)) });

int cost1 = ability1.ExpCost();

Ability ability2 = new();

ability2.AddEffect(new DamageEffect { Impact = new((2, 4)) });

int cost2 = ability1.ExpCost();


Console.ReadLine();