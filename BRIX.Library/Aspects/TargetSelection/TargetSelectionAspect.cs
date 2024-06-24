using BRIX.Library.Enums;
using BRIX.Library.Mathematics;
using System.Reflection.Metadata;

namespace BRIX.Library.Aspects.TargetSelection
{
    public class TargetSelectionAspect : AspectBase
    {
        public override void Initialize()
        {
            base.Initialize();
            TargetSelectionRestrictions.Conditions = [(ETargetSelectionRestrictions.SeeTarget, string.Empty)];
        }

        public override double GetCoefficient()
        {
            return Strategy switch
            {
                ETargetSelectionStrategy.Area => GetAreaCoeficient(),
                ETargetSelectionStrategy.NTargetsAtDistanсeL => GetNTADCoeficient(),
                _ => 1,
            };
        }

        public ETargetSelectionStrategy Strategy { get; set; } = ETargetSelectionStrategy.NTargetsAtDistanсeL;

        public NTADSettings NTAD { get; set; } = new ();

        public AreaSettings AreaSettings { get; set; } = new ();

        public TargetChainSettings TargetChain { get; set; } = new ();

        public TargetSizeSettings TargetsSizes { get; set; } = new ();

        public TargetSelectionRestrictionsSettings TargetSelectionRestrictions { get; set; } = new ();

        private double GetNTADCoeficient()
        {
            double distanceCoef = GetDistanceCoeficient(NTAD.DistanceInMeters);
            double countCoef = new ThrasholdCostConverter((1, 0), (2, 100), (6, 50), (11, 10), (101, 1))
                .Convert(NTAD.TargetsCount)
                .ToCoeficient();
            double randomSelectionCoef = NTAD?.IsTargetSelectionIsRandom == true ? 0.8 : 1;

            return distanceCoef * countCoef
                * ObstacleEquivalent.Map[NTAD?.ObstacleBetweenCharacterAndTarget ?? 0].ToCoeficient()
                * randomSelectionCoef
                * TargetChain.GetCoefficient()
                * TargetsSizes.GetCoefficient()
                * TargetSelectionRestrictions.GetCoefficient();
        }

        private double GetAreaCoeficient()
        {
            double distanceCoef = GetDistanceCoeficient(AreaSettings.DistanceToAreaInMeters);

            int volume = AreaSettings.Area.Shape.GetVolume() <= 1 ? 1 : AreaSettings.Area.Shape.GetVolume();
            double volumeCoef = ((volume - 1) * 90).ToCoeficient();

            if(AreaSettings.Area.Shape is VoxelArray voxelArray && voxelArray.IsArbitrary)
            {
                volumeCoef *= 4;
            }

            double excludedTargetsCoef = new ThrasholdCostConverter((0, 0), (1, 30), (6, 5))
                .Convert(AreaSettings?.ExcludedTargetsCount ?? 0)
                .ToCoeficient();
            double areaBoundedToCharacterCoef = AreaSettings?.IsAreaBoundedTo == true ? 1.7 : 1;

            return distanceCoef * volumeCoef * excludedTargetsCoef
                * ObstacleEquivalent.Map[AreaSettings?.ObstacleBetweenCharacterAndArea ?? 0].ToCoeficient()
                * ObstacleEquivalent.Map[AreaSettings?.ObstacleBetweenEpicenterAndTarget ?? 0].ToCoeficient()
                * areaBoundedToCharacterCoef
                * TargetChain.GetCoefficient()
                * TargetsSizes.GetCoefficient()
                * TargetSelectionRestrictions.GetCoefficient();
        }

        public static double GetDistanceCoeficient(int distance)
        {
            return new ThrasholdCostConverter((1, 0), (2, 20), (3, 10), (21, 5), (101, 2), (1001, 1))
                .Convert(distance)
                .ToCoeficient();
        }
    }
}
