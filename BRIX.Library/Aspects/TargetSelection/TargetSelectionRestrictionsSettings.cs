using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects.TargetSelection
{
    public class TargetSelectionRestrictionsSettings
    {
        public List<(ETargetSelectionRestrictions Type, string Comment)> Conditions { get; set; } = new ();

        public double GetCoefficient()
        {
            if (!Conditions.Any())
            {
                return 1;
            }

            ETargetSelectionRestrictions restriction = (ETargetSelectionRestrictions)(object)Conditions.First().Type;
            double coeficient = ConditionToCoeficientMap[restriction].ToCoeficient();

            foreach ((ETargetSelectionRestrictions Restriction, string Comment) condition in Conditions.Skip(1))
            {
                coeficient *= ConditionToCoeficientMap[condition.Restriction].ToCoeficient();
            }

            return coeficient;
        }

        public Dictionary<ETargetSelectionRestrictions, int> ConditionToCoeficientMap => new ()
        {
            { ETargetSelectionRestrictions.SeeTarget, -10 },
            { ETargetSelectionRestrictions.TargetSeesCharacter, -5 },
            { ETargetSelectionRestrictions.HearTarget, -15 },
            { ETargetSelectionRestrictions.TargetHearsCharacter, -10 },

            { ETargetSelectionRestrictions.BeAlive, -20 },
            { ETargetSelectionRestrictions.BeAnObject, -20 },
            { ETargetSelectionRestrictions.BeViable, -20 },

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

        BeAlive,
        BeAnObject,
        BeViable,

        LowRarityProperty,
        MediumRarityProperty,
        HighRarityProperty,
    }
}
