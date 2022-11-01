// See https://aka.ms/new-console-template for more information
using BRIX.Library;
using BRIX.Library.Aspects;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Library.Mathematics;

HashSet<Type> hashSet = new();

hashSet.Add(typeof(int));
hashSet.Add(typeof(int));

AspectBase aspect1 = new ActivationConditionsAspect();
ActivationConditionsAspect aspect2 = new ActivationConditionsAspect();
AspectBase aspect3 = new CooldownAspect();

hashSet.Add(aspect1.GetType());
hashSet.Add(aspect2.GetType());
hashSet.Add(aspect3.GetType());

Console.ReadLine();