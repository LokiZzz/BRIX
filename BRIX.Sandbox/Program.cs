using BRIX.Lexica;
using BRIX.Library.Effects;

EffectBase effect = new DamageEffect() { Impact = new(3, (2, 6)) };
string lexis = await effect.ToLexisAsync(); 

Console.ReadLine();
