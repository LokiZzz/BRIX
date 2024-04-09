using BRIX.Lexica;
using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Effects;
using BRIX.Library.Enums;
using BRIX.Library.Mathematics;

TargetSelectionAspect model = new ()
{
    Strategy = ETargetSelectionStrategy.Area,
    Area = new()
    {
        DistanceToAreaInMeters = 10,
        AreaType = EAreaType.Cone,
        ExcludedTargetsCount = 3,
        IsAreaBoundedTo = false,
        ObstacleBetweenCharacterAndArea = EObstacleEquivalent.WoodenPlank,
        ObstacleBetweenEpicenterAndTarget = EObstacleEquivalent.MetalArmor,
    },
    TargetChain = new()
    {
        IsChainEnabled = true,
        MaxDistanceBetweenTargets = 3,
        MaxTargetsCount = 4,
        ObstacleBetweenTargetsInChain = EObstacleEquivalent.DenseVegetation,
    },
    TargetSelectionRestrictions = new()
    {
        Conditions = 
        [
            (ETargetSelectionRestrictions.TargetSeesCharacter, string.Empty),
            (ETargetSelectionRestrictions.MediumRarityProperty, "цель должна быть эльфом")
        ]
    },
};

model.Area.GetConcreteShape<Cone>().H = 10;
model.Area.GetConcreteShape<Cone>().R = 5;

string? text = await model.ToLexis2();

Console.WriteLine(text);