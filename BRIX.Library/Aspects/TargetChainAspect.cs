using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects
{
    public class TargetChainAspect : AspectBase
    {
        public bool IsChainEnabled { get; set; }
        public int MaxDistanceBetweenTargets { get; set; }
        public int MaxTargetsCount { get; set; }

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
