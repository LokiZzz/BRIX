using BRIX.Library.Aspects;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public partial class TargetSelectionAspectModel : AspectModelBase
    {
        public TargetSelectionAspectModel() : this(new TargetSelectionAspect()) { }

        public TargetSelectionAspectModel(TargetSelectionAspect model)
        {
            InternalModel = model;
        }

        public TargetSelectionAspect Internal => GetSpecificAspect<TargetSelectionAspect>();
    }
}
