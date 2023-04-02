// See https://aka.ms/new-console-template for more information
using BRIX.Lexica;

while (true)
{
    int number = int.Parse(Console.ReadLine());
    Console.WriteLine(AspectLexis.Declension(number, "очко"));
}

