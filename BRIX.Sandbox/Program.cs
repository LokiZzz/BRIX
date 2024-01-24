// See https://aka.ms/new-console-template for more information
using BRIX.Lexica;
using BRIX.Library.Aspects;
using BRIX.Library.Effects;
using System.Globalization;

Console.WriteLine("Hello, World!");

ActionPointAspect model = new ActionPointAspect() { ActionPoints = 6 };

Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
string eng = model.ToLexis();
Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ru-RU");
string rus = model.ToLexis();

Console.WriteLine(rus);