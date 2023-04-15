using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects
{
    public class TargetChainAspect : AspectBase
    {
        public bool IsChainEnabled { get; set; }
        public int MaxDistanceBetweenTargets { get; set; } = 1;
        public int MaxTargetsCount { get; set; } = 2;

        public override double GetCoefficient()
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
