using BRIX.Library.Ability;
using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.Effects;
using BRIX.Library.Extensions;
using BRIX.Library.Mathematics;

Status status = new Status();
status.AddEffect(new FortifyEffect());
status.AddEffect(new ExhaustionEffect());

try
{
    status.AddEffect(new HealEffect());
}
catch (Exception ex)
{
    string here = "!!!";
}

status.RemoveEffect(status.Effects.FirstOrDefault(x => x is ExhaustionEffect));

Console.ReadLine();