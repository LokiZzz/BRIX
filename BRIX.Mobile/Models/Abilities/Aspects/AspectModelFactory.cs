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
                default: 
                    return null;
            }
        }
    }
}
