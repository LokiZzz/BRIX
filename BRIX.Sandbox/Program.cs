using BRIX.Lexica;
using BRIX.Library.Effects;
using BRIX.Sandbox;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

DamageEffect model = new DamageEffect() { Impact = new BRIX.Library.DiceValue.DicePool((3, 6)) };

string? text = model.ToLexis2();

Console.WriteLine(text);