using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects
{
    public class ObstacleAspect : AspectBase
    {
        public EObstacleEquivalent BetweenCharacterAndTarget { get; set; }
        public EObstacleEquivalent BetweenCharacterAndArea { get; set; }
        public EObstacleEquivalent BetweenEpicenterAndTarget { get; set; }
        public EObstacleEquivalent BetweenTargetsInChain { get; set; }
        public EObstacleEquivalent BetweenTargetAndDestinationPoint { get; set; }

        private Dictionary<EObstacleEquivalent, int> EquivalentToPercentMap => new ()
        {
            { EObstacleEquivalent.None, 0 },
            { EObstacleEquivalent.Paper, -35 },
            { EObstacleEquivalent.From1mmSteel, -30 },
            { EObstacleEquivalent.From5mmSteel, -25 },
            { EObstacleEquivalent.From10mmSteel, -20 },
            { EObstacleEquivalent.From100mmSteel, -15 },
            { EObstacleEquivalent.From1000mmSteel, -10 },
            { EObstacleEquivalent.MuchMorePowerfullObstacle, -5 },
        };

        public override double GetCoefficient()
        {
            return EquivalentToPercentMap[BetweenCharacterAndTarget].ToCoeficient()
                * EquivalentToPercentMap[BetweenCharacterAndArea].ToCoeficient()
                * EquivalentToPercentMap[BetweenEpicenterAndTarget].ToCoeficient()
                * EquivalentToPercentMap[BetweenTargetsInChain].ToCoeficient()
                * EquivalentToPercentMap[BetweenTargetAndDestinationPoint].ToCoeficient();
        }
    }

    public enum EObstacleEquivalent
    {
        None,
        Paper,
        From1mmSteel,
        From5mmSteel,
        From10mmSteel,
        From100mmSteel,
        From1000mmSteel,
        MuchMorePowerfullObstacle
    }
}
