using BRIX.Lexica;
using BRIX.Library.Aspects;

ActivationConditionsAspect model = new ActivationConditionsAspect()
{
    Conditions = new List<(EActivationCondition Type, string Comment)>
    {
        (EActivationCondition.NeedToAbleToTalk, string.Empty),
        (EActivationCondition.MediumActivationCondition, "Full moon at the line of sight"),
    },
};

string text = model.ToLexis();

Console.WriteLine(text);