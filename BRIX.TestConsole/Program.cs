// See https://aka.ms/new-console-template for more information
using BRIX.Library;
using BRIX.Library.DiceValue;
using BRIX.Library.Mathematics;

Console.WriteLine("Hello, World!");

ThrasholdCoefConverter converter = new((1, 0), (2, 20), (3, 10), (21, 5), (101, 2), (1001, 1));

int coef = converter.Convert(10000);
Console.WriteLine("Hello, World!");
