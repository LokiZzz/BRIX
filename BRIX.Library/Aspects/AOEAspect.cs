using BRIX.Library.Aspects.Base;
using BRIX.Library.Extensions;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects
{
    public class AOEAspect : AspectBase
    {
        public VolumeShape AreaShape { get; set; } = new();

        public int DistanceToArea { get; set; } = 1;

        /// <summary>
        /// Привязывается ли область действия к персонажу.
        /// </summary>
        public bool CanBeBounded { get; set; } = false;

        /// <summary>
        /// Огибает ли область углы.
        /// </summary>
        public bool SpreadsAroundCorners { get; set; } = false;

        public override double GetCoefficient()
        {
            double distanceCoef = new ThrasholdCostConverter((1, 0), (2, 20), (3, 10), (21, 5), (101, 2), (1001, 1))
                .Convert(DistanceToArea)
                .ToCoeficient();

            double volume = AreaShape.Shape.GetVolume() <= 1 ? 1 : AreaShape.Shape.GetVolume();
            double volumeCoef = ((volume - 1) * 25).Round().ToCoeficient();

            if (AreaShape.Shape is VoxelArray voxelArray && voxelArray.IsArbitrary)
            {
                volumeCoef *= 4;
            }

            double areaCanBeBoundedCoef = CanBeBounded == true ? 1.7 : 1;
            double spreadsAroundCornersCoef = SpreadsAroundCorners == true ? 1.5 : 1;

            return distanceCoef * volumeCoef * areaCanBeBoundedCoef * spreadsAroundCornersCoef;
        }
    }
}
