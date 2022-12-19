// See https://aka.ms/new-console-template for more information
using BRIX.Library;
using BRIX.Library.Aspects;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Library.Mathematics;
using System.Runtime.Intrinsics.Arm;

string input = string.Empty;

while(input != "stop")
{
    input = Console.ReadLine() ?? string.Empty;
    string[] splitted = input.Split('-');
    int from = int.Parse(splitted[0]);
    int to = int.Parse(splitted[1]);

    DicePool dicePool = DicePool.FromRange(from, to);

    Console.WriteLine(dicePool);
}