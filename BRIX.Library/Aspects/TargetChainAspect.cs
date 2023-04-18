using BRIX.Library.Enums;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects
{
    public class TargetChainAspect : AspectBase
    {
        public bool IsChainEnabled { get; set; }
        public int MaxDistanceBetweenTargets { get; set; } = 1;
        public int MaxTargetsCount { get; set; } = 2;
        public EObstacleEquivalent ObstacleBetweenTargetsInChain { get; set; }

        public override double GetCoefficient()
        {
            if (IsChainEnabled)
            {
                int obstacleCoef = EquivalentToPercentMap[ObstacleBetweenTargetsInChain];

                return ((75 + MaxDistanceBetweenTargets) * MaxTargetsCount * obstacleCoef).ToCoeficient();
            }
            else
            {
                return 1;
            }
        }

        private Dictionary<EObstacleEquivalent, int> EquivalentToPercentMap => new()
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
    }
}
