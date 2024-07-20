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

AbilityActivation activation = new()
{
    ActionPoints = 3,
    Cooldown = ECooldownOption.Minute,
    UsesCount = 2,
    Triggers = [(ETriggerProbability.Medium, "Триггер 1"), (ETriggerProbability.High, "Триггер 2")]
};

AbilityActivation activ2 = new();

Console.WriteLine(await activation.ToShortLexis());
Console.WriteLine(await activ2.ToShortLexis());

Console.ReadLine();