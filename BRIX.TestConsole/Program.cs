using BRIX.Library.Aspects;
using BRIX.Library.Characters;

while (true)
{
    Console.Clear();

    string input = Console.ReadLine();
    ECooldownOption period = (ECooldownOption)int.Parse(input.Split(' ')[0]);
    int uses = int.Parse(input.Split(' ')[1]);

    CooldownAspect aspect = new CooldownAspect();
    aspect.Condition = period;
    aspect.UsesCount = uses;

    Console.WriteLine($"Опция: {aspect.Condition:G} Использования: {aspect.UsesCount} Коэф: {aspect.GetCoefficient()}");

    Console.ReadLine();
}