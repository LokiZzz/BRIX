using BRIX.Lexica;
using BRIX.Library.Aspects;
using System.Globalization;

DurationAspect model = new DurationAspect()
{
    Duration = 6,
    Unit = BRIX.Library.Enums.ETimeUnit.Year,
    CanDisableStatus = true,
};

Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

string text = model.ToLexis();

Console.WriteLine(text);