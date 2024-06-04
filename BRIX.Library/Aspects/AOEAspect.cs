using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects
{
    public class AOEAspect : AspectBase
    {
        public VolumeShape AreaShape { get; set; } = new();

        public int DistanceToArea { get; set; } = 1;

        public bool CanBeBounded { get; set; } = false;

        public override double GetCoefficient()
        {
            double distanceCoef = new ThrasholdCostConverter((1, 0), (2, 20), (3, 10), (21, 5), (101, 2), (1001, 1))
                .Convert(DistanceToArea)
                .ToCoeficient();
            double volumeCoef = (AreaShape.Shape.GetVolume() * 5).ToCoeficient();
            double areaCanBeBoundedCoef = CanBeBounded == true ? 1.7 : 1;

            return distanceCoef * volumeCoef * areaCanBeBoundedCoef;
        }
    }
}
