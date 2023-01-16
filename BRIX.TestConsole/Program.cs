// See https://aka.ms/new-console-template for more information
using BRIX.Library;
using BRIX.Library.Aspects;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Library.Mathematics;
using System.Runtime.Intrinsics.Arm;

HealDamageEffect hdEffect = new() { Impact = new DicePool(1, (1, 4)) };
int cost = hdEffect.GetExpCost();
Console.ReadLine();