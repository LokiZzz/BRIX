using BRIX.Library.Aspects;

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
                case TargetChainAspect targetChainAspect:
                    return new TargetChainAspectModel(targetChainAspect);
                default: 
                    return null;
            }
        }
    }
}
