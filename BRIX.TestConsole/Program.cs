// See https://aka.ms/new-console-template for more information
using BRIX.Library;
using BRIX.Library.Character;
using BRIX.Library.DiceValue;
using BRIX.Library.Mathematics;


ThrasholdCoefConverter thrasholdConverter = new((1, 20), (2, 30), (3, 50), (4, 500), (7, 300), (21, 150), (101, 50));

List<int> ints = new() { 1, 2, 3, 4, 7, 25, 100 };

foreach(int elem in ints)
{
    Console.WriteLine(thrasholdConverter.Convert(elem));
}

Console.ReadLine();

