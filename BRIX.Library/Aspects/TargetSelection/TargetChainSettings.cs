using BRIX.Library.Enums;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects.TargetSelection
{
    public class TargetChainSettings
    {
        public bool IsChainEnabled { get; set; }
        public int MaxDistanceBetweenTargets { get; set; } = 1;
        public int MaxTargetsCount { get; set; } = 2;
        public EObstacleEquivalent ObstacleBetweenTargetsInChain { get; set; } = EObstacleEquivalent.MetalArmor;

        public double GetCoefficient()
        {
            if (IsChainEnabled)
            {
                double obstacleCoef = ObstacleEquivalent.Map[ObstacleBetweenTargetsInChain].ToCoeficient();

                return ((75 + MaxDistanceBetweenTargets) * MaxTargetsCount).ToCoeficient() * obstacleCoef;
            }
            else
            {
                return 1;
            }
        }
    }
}
