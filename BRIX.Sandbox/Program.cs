// See https://aka.ms/new-console-template for more information
using BRIX.Lexica;
using BRIX.Library.Aspects;
using BRIX.Library.Effects;
using System.Globalization;

Console.WriteLine("Hello, World!");

ActivationConditionsAspect model = new ActivationConditionsAspect()
{
    Conditions = new List<(Enum Type, string Comment)>
    {
        (EActivationCondition.NeedToAbleToTalk, string.Empty),
        (EActivationCondition.NeedToMove, string.Empty),
        (EActivationCondition.MediumActivationCondition, "Должна быть видна полная луна"),
    }
};

string text = model.ToLexis();

Console.WriteLine(text);