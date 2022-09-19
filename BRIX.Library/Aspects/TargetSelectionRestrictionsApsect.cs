namespace BRIX.Library.Aspects
{
    public class TargetSelectionRestrictionsApsect : MultiConditionalAspect<ETargetSelectionRestrictions>
    {
        public override Dictionary<ETargetSelectionRestrictions, int> ConditionToCoeficientMap => new Dictionary<ETargetSelectionRestrictions, int>
        {
            { ETargetSelectionRestrictions.SeeTarget, -10 },
            { ETargetSelectionRestrictions.TargetSeesCharacter, -5 },
            { ETargetSelectionRestrictions.HearTarget, -15 },
            { ETargetSelectionRestrictions.TargetHearsCharacter, -10 },
            { ETargetSelectionRestrictions.DetermineTargetSomehow, -20 },
            { ETargetSelectionRestrictions.TargetDeterminesCharacterSomehow, -25 },

            { ETargetSelectionRestrictions.BeOrganic, -20 },
            { ETargetSelectionRestrictions.BeNotOrganic, -20 },
            { ETargetSelectionRestrictions.HaveSoul, -20 },
            { ETargetSelectionRestrictions.HaveNoSoul, -20 },

            { ETargetSelectionRestrictions.LowRarityProperty, -10 },
            { ETargetSelectionRestrictions.MediumRarityProperty, -20 },
            { ETargetSelectionRestrictions.HighRarityProperty, -30 },
        };
    }

    public enum ETargetSelectionRestrictions
    {
        SeeTarget,
        TargetSeesCharacter,
        HearTarget,
        TargetHearsCharacter,
        DetermineTargetSomehow,
        TargetDeterminesCharacterSomehow,

        BeOrganic,
        BeNotOrganic,
        HaveSoul,
        HaveNoSoul,

        LowRarityProperty,
        MediumRarityProperty,
        HighRarityProperty,
    }
}
