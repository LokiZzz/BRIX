using BRIX.Lexica;
using BRIX.Library.Effects;

WinTheGameEffect model = new ();

string? text = await model.ToLexis2();

Console.WriteLine(text);