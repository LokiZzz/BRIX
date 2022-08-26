using BRIX.Library.Effects.Base;
using BRIX.Library.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Effects.HealDamage
{
    public class TargetSelectionRestrictionsApsect : MultiConditionalAspect<ETargetSelectionRestrictions>
    {
        public override Dictionary<ETargetSelectionRestrictions, int> ConditionToCoeficientMap => new Dictionary<ETargetSelectionRestrictions, int>
        {
            { ETargetSelectionRestrictions.NeedToSeeTarget, 10 },
            { ETargetSelectionRestrictions.NeedToTargetSees, 5 },

            { ETargetSelectionRestrictions.NeedToHearTarget, 15 },
            { ETargetSelectionRestrictions.NeedToTargetHears, 10 },

            { ETargetSelectionRestrictions.NeedToDetermineTarget, 20 },
            { ETargetSelectionRestrictions.NeedToTargetDetermines, 25 },

            { ETargetSelectionRestrictions.TargetMustBeOrganicOrNotOrganic, 20 },
            { ETargetSelectionRestrictions.TargetMustHaveSoul, 20 },

            { ETargetSelectionRestrictions.TargetMustHaveLowRarityProperty, 10 },
            { ETargetSelectionRestrictions.TargetMustHaveMediumRarityProperty, 20 },
            { ETargetSelectionRestrictions.TargetMustHaveHighRarityProperty, 30 },
        };
    }

    public enum ETargetSelectionRestrictions
    {
        NeedToSeeTarget = 10,
        NeedToTargetSees = 5,

        NeedToHearTarget = 15,
        NeedToTargetHears = 10,

        NeedToDetermineTarget = 20,
        NeedToTargetDetermines = 25,

        TargetMustBeOrganicOrNotOrganic = 20,
        TargetMustHaveSoul = 20,

        TargetMustHaveLowRarityProperty = 10,
        TargetMustHaveMediumRarityProperty = 20,
        TargetMustHaveHighRarityProperty = 30
    }
}
