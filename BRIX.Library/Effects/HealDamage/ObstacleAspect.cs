using BRIX.Library.Effects.Base;
using BRIX.Library.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Effects.HealDamage
{
    public class ObstacleAspect : AspectBase
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
        None = 0,
        Paper = 1,
        From1To3mmSteel = 2,
        From4To10mmSteel = 3,
        From11To100mmSteel = 4,
        From101To1000mmSteel = 5,
        MuchMorePowerfullObstacle = 6
    }
}
