using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.Effects;
using BRIX.Library.Extensions;
using BRIX.Library.Mathematics;

HealEffect heal = new HealEffect();

var healModel = EffectModelFactory.Create(heal);

Console.ReadLine();