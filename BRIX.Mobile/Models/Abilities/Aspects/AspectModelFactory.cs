using BRIX.Library.Ability;
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
                case TargetSelectionAspect tsa:
                    return new TargetSelectionAspectModel(tsa);
                case ActivationConditionsAspect aca:
                    return new ActivationConditionsAspectModel(aca);
                case DurationAspect rda:
                    return new DurationAspectModel(rda);
                default: 
                    throw new NotImplementedException($"Для аспекта {aspect.GetType()} не реализована модель.");
            }
        }
    }
}
