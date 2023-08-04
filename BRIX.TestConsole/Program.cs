using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.Extensions;
using BRIX.Library.Mathematics;

ThrasholdCostConverter converter = new ThrasholdCostConverter((1, 100), (6, 50), (11, 25));

for(int i = 1; i < 12; i++)
{
    Console.WriteLine(converter.Convert(i));
}

Console.ReadLine();