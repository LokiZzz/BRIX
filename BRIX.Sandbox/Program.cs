using BRIX.Library.Abilities;
using BRIX.Library.Characters;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;

NPC goblin = new() { Name = "Мусорный гоблин", Health = 7 };
Ability stab = new() { Name = "Пырнуть ножом" };
stab.AddEffect(new DamageEffect() { Impact = new DicePool((1, 4)) });
stab.AddEffect(new ExhaustionEffect() { Impact = new DicePool(5) });
goblin.Abilities.Add(stab);

int goblinPower = goblin.Power;

Console.WriteLine(goblinPower);