// See https://aka.ms/new-console-template for more information
using BRIX.Library;
using BRIX.Library.Aspects;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Library.Mathematics;

Ability smash = new Ability();
EffectBase damage = new HealDamageEffect();
smash.AddEffect(damage);

Console.ReadLine();