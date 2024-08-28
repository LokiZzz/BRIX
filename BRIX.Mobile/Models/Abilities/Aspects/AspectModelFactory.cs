using BRIX.Library.Aspects;
using BRIX.Library.Aspects.Base;
using BRIX.Library.Aspects.TargetSelection;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public static class AspectModelFactory
    {
        public static AspectModelBase GetAspectModel(AspectBase aspect)
        {
            return aspect switch
            {
                TargetSelectionAspect tsa => new TargetSelectionAspectModel(tsa),
                TargetSizeAspect tsize => new TargetSizeAspectModel(tsize),
                ActivationConditionsAspect aca => new ActivationConditionsAspectModel(aca),
                DurationAspect da => new DurationAspectModel(da),
                AOEAspect aoea => new AOEAspectModel(aoea),
                VampirismAspect va => new VampirismAspectModel(va),
                _ => throw new NotImplementedException($"Для аспекта {aspect.GetType()} не реализована модель."),
            };
        }
    }
}
