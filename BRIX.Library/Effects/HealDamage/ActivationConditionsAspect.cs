using BRIX.Library.Effects.Base;
using BRIX.Library.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Effects.HealDamage
{
    public class ActivationConditionsAspect : MultiConditionalAspect<EActivationCondition>
    {
        public override Dictionary<EActivationCondition, int> ConditionToCoeficientMap => new Dictionary<EActivationCondition, int>
        {
            { EActivationCondition.NeedToMove, -5 },
            { EActivationCondition.NeedToMoveArm, -10 },
            { EActivationCondition.NeedToMoveBothArms, -15 },
            { EActivationCondition.NeedToAbleToTalk, -10 },

            { EActivationCondition.EasyCondition, -10 },
            { EActivationCondition.MediumCondition, -20 },
            { EActivationCondition.HardCondition, -40 }
        };
    }

    public enum EActivationCondition
    {
        NeedToMove = 1,
        NeedToMoveArm = 2,
        NeedToMoveBothArms = 3,
        NeedToAbleToTalk = 4,

        EasyCondition = 5,
        MediumCondition = 6,
        HardCondition = 7
    }
}
