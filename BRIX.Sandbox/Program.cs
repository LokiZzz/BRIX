using BRIX.Lexica;
using BRIX.Library.Effects;

MoveTargetEffect model = new ()
{
    TargetPath = EMoveTargetPath.NoPath,
    DistanceInMeters = 15
};

string? text = await model.ToLexis2();

Console.WriteLine(text);