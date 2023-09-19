using BRIX.Library.Ability;
using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.Effects;
using BRIX.Library.Extensions;
using BRIX.Library.Mathematics;

while (true)
{
    int duration = int.Parse(Console.ReadLine());
    DurationAspect aspect = new DurationAspect();
    
    aspect.Duration = duration;
    Console.WriteLine($"Coef: {aspect.GetCoefficient()}");
}