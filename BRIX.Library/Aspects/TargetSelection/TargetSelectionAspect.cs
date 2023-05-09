using BRIX.Library.Enums;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects.TargetSelection
{
    public class TargetSelectionAspect : AspectBase
    {
        public override double GetCoefficient()
        {
            switch (Strategy)
            {
                    
                case ETargetSelectionStrategy.Area:
                    return GetAreaCoeficient();
                case ETargetSelectionStrategy.NTargetsAtDistanсeL:
                    return GetNTADCoeficient();
                case ETargetSelectionStrategy.CharacterHimself:
                default:
                    return 1;
            }
        }

        public ETargetSelectionStrategy Strategy { get; set; } = ETargetSelectionStrategy.NTargetsAtDistanсeL;

        public NTADSettings NTAD { get; set; } = new NTADSettings();

        public AreaSettings Area { get; set; } = new AreaSettings();

        public TargetChainSettings TargetChain { get; set; } = new TargetChainSettings();

        public TargetSizeSettings TargetsSizes { get; set; } = new TargetSizeSettings();

        public TargetSelectionRestrictionsSettings TargetSelectionRestrictions { get; set; } = new TargetSelectionRestrictionsSettings();

        private double GetNTADCoeficient()
        {
            double distanceCoef = GetDistanceCoeficient(NTAD.DistanceInMeters);
            double countCoef = new ThrasholdCoefConverter((1, 0), (2, 100), (6, 50), (11, 10), (101, 1))
                .Convert(NTAD.TargetsCount)
                .ToCoeficient();
            double randomSelectionCoef = NTAD?.IsTargetSelectionIsRandom == true ? 0.8 : 1;

            return distanceCoef * countCoef
                * EquivalentToPercentMap[NTAD?.ObstacleBetweenCharacterAndTarget ?? 0].ToCoeficient()
                * randomSelectionCoef
                * TargetChain.GetCoefficient()
                * TargetsSizes.GetCoefficient()
                * TargetSelectionRestrictions.GetCoefficient();
        }

        private double GetAreaCoeficient()
        {
            double distanceCoef = GetDistanceCoeficient(Area.DistanceToAreaInMeters);
            double volumeCoef = (Area?.Shape?.GetVolume() ?? 0 * 5).ToCoeficient();
            double excludedTargetsCoef = new ThrasholdCoefConverter((0, 0), (1, 30), (6, 5))
                .Convert(Area?.ExcludedTargetsCount ?? 0)
                .ToCoeficient();

            return distanceCoef * volumeCoef * excludedTargetsCoef
                * EquivalentToPercentMap[Area?.ObstacleBetweenCharacterAndArea ?? 0].ToCoeficient()
                * EquivalentToPercentMap[Area?.ObstacleBetweenEpicenterAndTarget ?? 0].ToCoeficient()
                * TargetChain.GetCoefficient()
                * TargetsSizes.GetCoefficient()
                * TargetSelectionRestrictions.GetCoefficient();
        }

        private double GetDistanceCoeficient(int distance)
        {
            return new ThrasholdCoefConverter((1, 0), (2, 20), (3, 10), (21, 5), (101, 2), (1001, 1))
                .Convert(distance)
                .ToCoeficient();
        }

        private Dictionary<EObstacleEquivalent, int> EquivalentToPercentMap => new()
        {
            { EObstacleEquivalent.None, 0 },
            { EObstacleEquivalent.PaperSheet, -50 },
            { EObstacleEquivalent.DenseVegetation, -25 },
            { EObstacleEquivalent.LeatherArmor, -10 },
            { EObstacleEquivalent.WoodenPlank, 0 },
            { EObstacleEquivalent.MetalArmor, 100 },
            { EObstacleEquivalent.BrickWall, 500 },
            { EObstacleEquivalent.ThickSteelPlate, 1000 },
            { EObstacleEquivalent.MuchMorePowerfullObstacle, 5000 },
        };
    }
}
