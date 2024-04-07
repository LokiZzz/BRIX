using BRIX.Lexica;
using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Effects;

TargetSelectionAspect model = new ()
{
    Strategy = ETargetSelectionStrategy.NTargetsAtDistanсeL,
    NTAD = new()
    {
         DistanceInMeters = 15,
         TargetsCount = 5,
         ObstacleBetweenCharacterAndTarget = BRIX.Library.Enums.EObstacleEquivalent.None
    }
};

string? text = await model.ToLexis2();

Console.WriteLine(text);