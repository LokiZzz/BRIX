namespace BRIX.Library.Aspects
{
    public class ActivationConditionsAspect : MultiConditionalAspect<EActivationCondition>
    {
        public override void Initialize()
        {
            base.Initialize();
            Conditions = [(EActivationCondition.AbleToMove, string.Empty)];
        }

        public override Dictionary<EActivationCondition, int> ConditionToCoeficientMap => new ()
        {
            { EActivationCondition.AbleToMoveOneArm, -10 },
            { EActivationCondition.AbleToMoveBothArms, -15 },
            { EActivationCondition.AbleToTalk, -15 },
            { EActivationCondition.AbleToMove, -20 },

            { EActivationCondition.EasyActivationCondition, -20 },
            { EActivationCondition.MediumActivationCondition, -40 },
            { EActivationCondition.HardActivationCondition, -60 }
        };
    }

    public enum EActivationCondition
    {
        AbleToMove = 1,
        AbleToMoveOneArm = 2,
        AbleToMoveBothArms = 3,
        AbleToTalk = 4,

        EasyActivationCondition = 5,
        MediumActivationCondition = 6,
        HardActivationCondition = 7
    }
}
