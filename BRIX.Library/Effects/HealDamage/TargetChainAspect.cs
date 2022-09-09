using BRIX.Library.Effects.Base;
using BRIX.Library.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Effects.HealDamage
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
