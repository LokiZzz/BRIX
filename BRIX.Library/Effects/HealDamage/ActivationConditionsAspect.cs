using BRIX.Library.Effects.Base;
using BRIX.Library.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Effects.HealDamage
{
    public class ActivationConditionsAspect : AspectBase
    {
        public List<EActivationCodition> Conditions { get; set; } = new List<EActivationCodition>();

        public override double GetCoefficient()
        {
            if (!Conditions.Any())
            {
                return 1;
            }

            double coeficient = ((int)Conditions.First()).ToNegativeCoeficient();

            foreach (EActivationCodition condition in Conditions.Skip(1))
            {
                coeficient *= ConditionToCoefMap[condition].ToNegativeCoeficient();
            }

            return coeficient;
        }

        public Dictionary<EActivationCodition, int> ConditionToCoefMap = new Dictionary<EActivationCodition, int>
        {
            { EActivationCodition.NeedToMove, 5 },
            { EActivationCodition.NeedToMoveArm, 10 },
            { EActivationCodition.NeedToMoveBothArms, 15 },
            { EActivationCodition.NeedToAbleToTalk, 10 },
            { EActivationCodition.EasyCondition, 10 },
            { EActivationCodition.MediumCondition, 20 },
            { EActivationCodition.HardCondition, 40 }
        };
    }

    public enum EActivationCodition
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
