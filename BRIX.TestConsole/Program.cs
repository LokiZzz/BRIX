using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.Extensions;

//while (true)
//{
//    Console.Clear();

//    string input = Console.ReadLine();
//    ECooldownOption period = (ECooldownOption)int.Parse(input.Split(' ')[0]);
//    int uses = int.Parse(input.Split(' ')[1]);

//    CooldownAspect aspect = new CooldownAspect();
//    aspect.Condition = period;
//    aspect.UsesCount = uses;

//    Console.WriteLine($"Опция: {aspect.Condition:G} Использования: {aspect.UsesCount} Коэф: {aspect.GetCoefficient()}");

//    Console.ReadLine();
//}

IEnumerable<TestModel> enumerable = new List<TestModel>() { new TestModel { Prop = 1 }, new TestModel { Prop = 2 } };

enumerable.Replace(enumerable.First(), new TestModel { Prop = 3 });

Console.ReadLine();

public class TestModel { public int Prop { get; set; } }