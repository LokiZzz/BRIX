// See https://aka.ms/new-console-template for more information
using BRIX.Lexica;
using BRIX.Library;
using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Library.Mathematics;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Text.Json;

while (true)
{
    int number = int.Parse(Console.ReadLine());
    Console.WriteLine(AspectLexis.Declension(number, "очко"));
}

