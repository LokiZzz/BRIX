// See https://aka.ms/new-console-template for more information
using BRIX.Web.Shared.Json;
using System.Text.Json;

Console.WriteLine("Hello, World!");

JsonSerializerOptions deserializeOptions = new ();
deserializeOptions.Converters.Add(new PolymorphicJsonConverterFactory());

List<A> polymorphicList = [new B(), new C()];
string json = JsonSerializer.Serialize(polymorphicList, deserializeOptions);
polymorphicList = JsonSerializer.Deserialize<List<A>>(json) ?? throw new Exception();

Console.WriteLine(json);

public abstract class A
{
    public string PropA { get; set; } = "A";
}

public class B : A
{
    public string PropB { get; set; } = "B";
}

public class C : A
{
    public string PropC { get; set; } = "C";
}