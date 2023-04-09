// See https://aka.ms/new-console-template for more information

using BRIX.Library;
using BRIX.Library.Aspects;
using BRIX.Library.Effects;

Console.ReadLine();

Ability ability = new Ability();
DamageEffect effect = new DamageEffect();
TargetSelectionAspect targetSelectionAspect = new TargetSelectionAspect();
effect.SetAspect(targetSelectionAspect);
ability.AddEffect(effect);
int cost = ability.ExpCost();

Console.ReadLine();