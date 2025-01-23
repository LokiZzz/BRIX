using BRIX.Library.Aspects.Base;
using BRIX.Library.Extensions;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects.TargetSelection
{
    public class TargetSelectionAspect : AspectBase
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        public override double GetCoefficient()
        {
            return Strategy switch
            {
                ETargetSelectionStrategy.Area => GetAreaCoeficient(),
                ETargetSelectionStrategy.NTargetsAtDistanсeL => GetNTADCoeficient(),
                _ => 0.75,
            };
        }

        public ETargetSelectionStrategy Strategy { get; set; } = ETargetSelectionStrategy.NTargetsAtDistanсeL;

        public NTADSettings NTAD { get; set; } = new ();

        public AreaSettings AreaSettings { get; set; } = new ();

        public TargetChainSettings TargetChain { get; set; } = new ();

        public bool NeedToSeeTarget { get; set; } = true;

        private double GetNTADCoeficient()
        {
            double distanceCoef = GetDistanceCoeficient(NTAD.DistanceInMeters);
            double countCoef = new ThrasholdCostConverter((1, 0), (2, 100), (6, 50), (11, 10), (101, 1))
                .Convert(NTAD.TargetsCount)
                .ToCoeficient();
            double randomSelectionCoef = NTAD?.IsTargetSelectionIsRandom == true ? 0.8 : 1;
            double needToSeeTargetCoef = NeedToSeeTarget == true ? 0.8 : 1;

            return distanceCoef * countCoef
                * randomSelectionCoef
                * needToSeeTargetCoef
                * TargetChain.GetCoefficient();
        }

        private double GetAreaCoeficient()
        {
            double distanceCoef = GetDistanceCoeficient(AreaSettings.DistanceToAreaInMeters);

            double volume = AreaSettings.Area.Shape.GetVolume() <= 1 ? 1 : AreaSettings.Area.Shape.GetVolume();
            // +20% за каждый квадратный метр объёма области, так как область менее эффективна, чем NTAD.
            double volumeCoef = ((volume - 1) * 20).Round().ToCoeficient(); 

            if(AreaSettings.Area.Shape is VoxelArray voxelArray && voxelArray.IsArbitrary)
            {
                volumeCoef *= 2;
            }

            double excludedTargetsCoef = new ThrasholdCostConverter((0, 0), (1, 30), (6, 5))
                .Convert(AreaSettings?.ExcludedTargetsCount ?? 0)
                .ToCoeficient();
            double needToSeeTargetCoef = NeedToSeeTarget == true ? 0.8 : 1;
            double spreadsAroundCornersCoef = AreaSettings?.SpreadsAroundCorners == true ? 1.5 : 1;

            return distanceCoef 
                * volumeCoef 
                * excludedTargetsCoef
                * needToSeeTargetCoef
                * spreadsAroundCornersCoef
                * TargetChain.GetCoefficient();
        }

        public static double GetDistanceCoeficient(int distance)
        {
            return new ThrasholdCostConverter((1, 0), (2, 20), (3, 10), (21, 5), (101, 2), (1001, 1))
                .Convert(distance)
                .ToCoeficient();
        }
    }
}
