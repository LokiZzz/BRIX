using BRIX.Library.Aspects;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public partial class TargetSelectionAspectModel : AspectModelBase
    {
        public TargetSelectionAspectModel(AspectBase model) : base(model) { }

        public TargetSelectionAspect Internal => GetSpecificAspect<TargetSelectionAspect>();

        public ETargetSelectionStrategy Strategy
        {
            get => Internal.Strategy;
            set 
            { 
                SetProperty(Internal.Strategy, value, Internal, (model, prop) => model.Strategy = prop);
                UpdateCost();
            }
        }

        public int NTADCount
        {
            get => Internal.NTAD.TargetsCount;
            set
            {
                SetProperty(Internal.NTAD.TargetsCount, value, Internal, (model, prop) => model.NTAD.TargetsCount = prop); 
                UpdateCost();
            }
        }

        public int NTADistance
        {
            get => Internal.NTAD.DistanceInMeters;
            set 
            { 
                SetProperty(Internal.NTAD.DistanceInMeters, value, Internal, (model, prop) => model.NTAD.DistanceInMeters = prop); 
                UpdateCost();
            }
        }
    }
}