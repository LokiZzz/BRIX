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

            int volume = AreaShape.Shape.GetVolume() <= 1 ? 1 : AreaShape.Shape.GetVolume();
            double volumeCoef = ((volume - 1) * 25).ToCoeficient();

            if (AreaShape.Shape is VoxelArray voxelArray && voxelArray.IsArbitrary)
            {
                volumeCoef *= 4;
            }

            double areaCanBeBoundedCoef = CanBeBounded == true ? 1.7 : 1;

            return distanceCoef * volumeCoef * areaCanBeBoundedCoef;
        }
    }
}
