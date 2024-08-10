using BRIX.Library.Enums;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects
{
    public class DurationAspect : AspectBase
    {
        public double DisableStatusCoef => CanDisableStatus ? 1.3 : 1;

        public int Duration { get; set; } = 1;

        public bool CanDisableStatus { get; set; } = false;

        public override double GetCoefficient()
        {
            double coef = new ThrasholdCostConverter((1, 0), (2, 100), (6, 50), (11, 25))
                .Convert(Duration)
                .ToCoeficient();

            return coef * DisableStatusCoef;
        }
    }
}
