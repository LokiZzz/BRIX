using BRIX.Library.Effects.Base;
using BRIX.Library.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Effects.HealDamage
{
    public class TargetSelectionRestrictionsApsect : AspectBase
    {
        public List<ETargetSelectionRestrictions> Restrictions { get; set; } = new List<ETargetSelectionRestrictions>();

        public override double GetCoefficient()
        {
            if(!Restrictions.Any())
            {
                return 1;
            }

            double coeficient = ((int)Restrictions.First()).ToNegativeCoeficient();

            foreach(ETargetSelectionRestrictions restriction in Restrictions.Skip(1))
            {
                coeficient *= ((int)restriction).ToNegativeCoeficient();
            }

            return coeficient;
        }
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
