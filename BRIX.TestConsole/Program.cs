// See https://aka.ms/new-console-template for more information
using BRIX.Library;
using BRIX.Library.Aspects;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Library.Mathematics;
using System.Runtime.Intrinsics.Arm;

DicePool breakedDown = DicePool.FromValue(40, 0.25);

string input = string.Empty;

while(input != "stop")
{
    input = Console.ReadLine() ?? string.Empty;
    bool parsed = DicePool.TryParse(input, out DicePool dp);

    Console.WriteLine($"Parsed: {parsed}\nInput: {input}\nToString(): {dp}");
}