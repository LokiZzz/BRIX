using BRIX.Library.Enums;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects
{
    public class DurationAspect : AspectBase
    {
        private double DisableStatusCoef => CanDisableStatus ? 1.3 : 1;

        public int Duration { get; set; } = 1;

        public ETimeUnit Unit { get; set; } = ETimeUnit.Round;

        public int DurationInSeconds => TimeUnitValue[Unit];

        public bool CanDisableStatus { get; set; } = false;

        public override double GetCoefficient()
        {
            ThrasholdCostConverter converter = new((1, 0), (2, 100), (6, 50), (11, 25));
            int percent = converter.Convert(Duration);
            double coef = percent.ToCoeficient() * TimeUnitValue[Unit] / 5;

            return coef * DisableStatusCoef;
        }

        private readonly Dictionary<ETimeUnit, int> TimeUnitValue = new()
        {
            { ETimeUnit.Round, 5 },
            { ETimeUnit.Minute, 60 },
            { ETimeUnit.Hour, 60 * 60 },
            { ETimeUnit.Day, 60 * 60 * 24 },
            { ETimeUnit.Year, 60 * 60 * 24 * 365 },
        };
    }
}
