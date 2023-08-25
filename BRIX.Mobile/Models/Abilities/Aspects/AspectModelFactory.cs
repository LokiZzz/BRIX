using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public static class AspectModelFactory
    {
        public static AspectModelBase GetAspectModel(AspectBase aspect)
        {
            switch(aspect)
            {
                case ActionPointAspect apa:
                    return new ActionPointsAspectModel(apa);
                case TargetSelectionAspect tsa:
                    return new TargetSelectionAspectModel(tsa);
                case CooldownAspect ca:
                    return new CooldownAspectModel(ca);
                case ActivationConditionsAspect aca:
                    return new ActivationConditionsAspectModel(aca);
                case RoundDurationAspect rda:
                    return new RoundDurationAspectModel(rda);
                default: 
                    return null;
            }
        }
    }
}
