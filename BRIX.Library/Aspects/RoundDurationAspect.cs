using BRIX.Library.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Aspects
{
    public class RoundDurationAspect : AspectBase
    {
        private double _disableStatusCoef => CanDisableStatus ? 1.3 : 1;

        public int Rounds { get; set; } = 1;

        public Status Status { get; set; } = new();

        public bool CanDisableStatus { get; set; } = false;

        public override double GetCoefficient()
        {
            ThrasholdCostConverter converter = new((1, 0), (2, 100), (6, 50), (11, 25));
            double roundsCoef = converter.Convert(Rounds).ToCoeficient();

            return roundsCoef * _disableStatusCoef;
        }
    }
}
