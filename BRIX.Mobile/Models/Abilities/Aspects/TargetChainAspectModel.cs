using BRIX.Library.Aspects;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public class TargetChainAspectModel : AspectModelBase
    {
        public TargetChainAspectModel(AspectBase model) : base(model) { }

        public TargetChainAspect Internal => GetSpecificAspect<TargetChainAspect>();

        public bool IsEnabled
        {
            get => Internal.IsChainEnabled;
            set
            {
                SetProperty(Internal.IsChainEnabled, value, Internal, 
                    (model, prop) => model.IsChainEnabled = prop);
                UpdateCost();
            }
        }

        public int MaxDistanceBetweenTargets
        {
            get => Internal.MaxDistanceBetweenTargets;
            set
            {
                SetProperty(Internal.MaxDistanceBetweenTargets, value, Internal, 
                    (model, prop) => model.MaxDistanceBetweenTargets = prop);
                UpdateCost();
            }
        }

        public int MaxTargetsCount
        {
            get => Internal.MaxTargetsCount;
            set
            {
                SetProperty(Internal.MaxTargetsCount, value, Internal, 
                    (model, prop) => model.MaxTargetsCount = prop);
                UpdateCost();
            }
        }
    }
}
