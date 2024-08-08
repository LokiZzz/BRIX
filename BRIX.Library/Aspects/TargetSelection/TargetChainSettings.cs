using BRIX.Library.Enums;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects.TargetSelection
{
    public class TargetChainSettings
    {
        public bool IsChainEnabled { get; set; }
        public int MaxDistanceBetweenTargets { get; set; } = 1;
        public int MaxTargetsCount { get; set; } = 2;

        public double GetCoefficient()
        {
            if (IsChainEnabled)
            {
                return ((75 + MaxDistanceBetweenTargets) * MaxTargetsCount).ToCoeficient();
            }
            else
            {
                return 1;
            }
        }
    }
}
