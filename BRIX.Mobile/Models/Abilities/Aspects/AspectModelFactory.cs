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
                case ActionPointAspect actionPointAspect:
                    return new ActionPointsAspectModel(actionPointAspect);
                case TargetSelectionAspect targetSelectionAspect:
                    return new TargetSelectionAspectModel(targetSelectionAspect);
                case CooldownAspect cooldownAspect:
                    return new CooldownAspectModel(cooldownAspect);
                default: 
                    return null;
            }
        }
    }
}
