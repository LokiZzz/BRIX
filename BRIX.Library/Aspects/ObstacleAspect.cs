using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects
{
    public class ObstacleAspect : FreeConcordanceAspect
    {
        public EObstacleEquivalent BetweenCharacterAndTargetOrArea { get; set; }
        public EObstacleEquivalent BetweenEpicenterAndTarget { get; set; }
        public EObstacleEquivalent ObstacleBetweenTargetsInChain { get; set; }

        private Dictionary<EObstacleEquivalent, int> ObstacleEquivalentToPercentMap => new Dictionary<EObstacleEquivalent, int>
        {
            { EObstacleEquivalent.None, 0 },
            { EObstacleEquivalent.Paper, -30 },
            { EObstacleEquivalent.From1To3mmSteel, -25 },
            { EObstacleEquivalent.From4To10mmSteel, -20 },
            { EObstacleEquivalent.From11To100mmSteel, -15 },
            { EObstacleEquivalent.From101To1000mmSteel, -10 },
            { EObstacleEquivalent.MuchMorePowerfullObstacle, -5 },
        };

        public override double GetCoefficient()
        {
            return ObstacleEquivalentToPercentMap[BetweenCharacterAndTargetOrArea].ToCoeficient()
                * ObstacleEquivalentToPercentMap[BetweenEpicenterAndTarget].ToCoeficient()
                * ObstacleEquivalentToPercentMap[ObstacleBetweenTargetsInChain].ToCoeficient();
        }
    }

    public enum EObstacleEquivalent
    {
        None,
        Paper,
        From1To3mmSteel,
        From4To10mmSteel,
        From11To100mmSteel,
        From101To1000mmSteel,
        MuchMorePowerfullObstacle
    }
}
