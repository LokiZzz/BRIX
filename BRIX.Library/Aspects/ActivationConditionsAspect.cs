namespace BRIX.Library.Aspects
{
    public class ActivationConditionsAspect : MultiConditionalAspect<EActivationCondition>
    {
        public override Dictionary<EActivationCondition, int> ConditionToCoeficientMap => new ()
        {
            { EActivationCondition.NeedToMoveArm, -10 },
            { EActivationCondition.NeedToMoveBothArms, -15 },
            { EActivationCondition.NeedToAbleToTalk, -15 },
            { EActivationCondition.NeedToMove, -20 },

            { EActivationCondition.EasyActivationCondition, -20 },
            { EActivationCondition.MediumActivationCondition, -40 },
            { EActivationCondition.HardActivationCondition, -60 }
        };
    }

    public enum EActivationCondition
    {
        NeedToMove = 1,
        NeedToMoveArm = 2,
        NeedToMoveBothArms = 3,
        NeedToAbleToTalk = 4,

        EasyActivationCondition = 5,
        MediumActivationCondition = 6,
        HardActivationCondition = 7
    }
}
