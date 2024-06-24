using BRIX.Library.Enums;

namespace BRIX.Library.Aspects.TargetSelection
{
    public static class ObstacleEquivalent
    {
        public static Dictionary<EObstacleEquivalent, int> Map => new()
        {
            { EObstacleEquivalent.None, 0 },
            { EObstacleEquivalent.PaperSheet, -50 },
            { EObstacleEquivalent.DenseVegetation, -25 },
            { EObstacleEquivalent.LeatherArmor, -10 },
            { EObstacleEquivalent.WoodenPlank, 0 },
            { EObstacleEquivalent.MetalArmor, 20 },
            { EObstacleEquivalent.BrickWall, 200 },
            { EObstacleEquivalent.ThickSteelPlate, 400 },
            { EObstacleEquivalent.MuchMorePowerfullObstacle, 1000 },
        };
    }
}
