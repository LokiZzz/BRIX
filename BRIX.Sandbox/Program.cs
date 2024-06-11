using BRIX.Library.DiceValue;

while (true)
{
    string? input = Console.ReadLine();
    Console.Clear();

    bool parsed = DicePool.TryParse(input ?? string.Empty, out DicePool? dice);

    if (parsed && dice != null)
    {
        Console.WriteLine($"{dice}");
        Console.WriteLine($"Avg: {dice.Average()}");
        Console.WriteLine($"Min: {dice.Min()}");
        Console.WriteLine($"Max: {dice.Max()}");
    }
    else
    {
        Console.WriteLine($"FAIL!!!");
    }
}