// See https://aka.ms/new-console-template for more information
using BRIX.Library;
using BRIX.Library.Aspects;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Library.Mathematics;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.Intrinsics.Arm;
using System.Text.Json;

ContainerObject container = new()
{
    Items = new List<BaseItem> 
    {
        new Item1 { Name = "Item1", Prop1 = "Prop1" }, 
        new Item2 { Name = "Item2", Prop2 = "Prop2" },
    }
};

string jsonMicrosoft = System.Text.Json.JsonSerializer.Serialize(container, 
    options: new JsonSerializerOptions 
    { 
        WriteIndented = true, 
        IncludeFields = true 
    }
);

string jsonNewtonsoft = JsonConvert.SerializeObject(container, 
    settings: new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
    }
);

Console.ReadLine();

public class ContainerObject
{
    public List<BaseItem> Items;
}

public class BaseItem
{
    public string Name { get; set; }
}

public class Item1 : BaseItem
{
    public string Prop1 { get; set; }
}

public class Item2 : BaseItem
{
    public string Prop2 { get; set; }
}