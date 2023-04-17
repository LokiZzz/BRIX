using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects
{
    public class ObstacleAspect : AspectBase
    {
        public EObstacleEquivalent BetweenCharacterAndTarget { get; set; }
        public EObstacleEquivalent BetweenCharacterAndArea { get; set; }
        public EObstacleEquivalent BetweenEpicenterAndTarget { get; set; }
        public EObstacleEquivalent BetweenTargetsInChain { get; set; }
        public EObstacleEquivalent BetweenTargetAndDestinationPoint { get; set; }

        private Dictionary<EObstacleEquivalent, int> EquivalentToPercentMap => new ()
        {
            { EObstacleEquivalent.None, 0 },
            { EObstacleEquivalent.PaperSheet, -50 },
            { EObstacleEquivalent.DenseVegetation, -25 },
            { EObstacleEquivalent.LeatherArmor, -10 },
            { EObstacleEquivalent.WoodenPlank, 0 },
            { EObstacleEquivalent.MetalArmor, 100 },
            { EObstacleEquivalent.BrickWall, 500 },
            { EObstacleEquivalent.ThickSteelPlate, 1000 },
            { EObstacleEquivalent.MuchMorePowerfullObstacle, 5000 },
        };

        public override double GetCoefficient()
        {
            return EquivalentToPercentMap[BetweenCharacterAndTarget].ToCoeficient()
                * EquivalentToPercentMap[BetweenCharacterAndArea].ToCoeficient()
                * EquivalentToPercentMap[BetweenEpicenterAndTarget].ToCoeficient()
                * EquivalentToPercentMap[BetweenTargetsInChain].ToCoeficient()
                * EquivalentToPercentMap[BetweenTargetAndDestinationPoint].ToCoeficient();
        }
    }

    public enum EObstacleEquivalent
    {
        None = 0,
        PaperSheet,
        DenseVegetation,
        LeatherArmor,
        WoodenPlank,
        MetalArmor,
        BrickWall,
        ThickSteelPlate,
        MuchMorePowerfullObstacle
    }
}
