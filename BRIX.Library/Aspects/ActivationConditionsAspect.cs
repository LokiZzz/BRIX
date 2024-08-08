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
            { EActivationCondition.AbleToTalk, -20 },
            { EActivationCondition.AbleToMove, -20 },

            { EActivationCondition.EasyActivationCondition, -25 },
            { EActivationCondition.MediumActivationCondition, -50 },
            { EActivationCondition.HardActivationCondition, -75 }
        };
    }

    public enum EActivationCondition
    {
        AbleToMove = 1,
        AbleToTalk = 2,

        EasyActivationCondition = 3,
        MediumActivationCondition = 4,
        HardActivationCondition = 5
    }
}
