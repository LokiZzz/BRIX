using BRIX.Library.Aspects;
using BRIX.Mobile.ViewModel.Abilities;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public partial class ActionPointsAspectModel : AspectModelBase
    {
        public ActionPointsAspectModel(AspectBase model) : base(model) { }

        public ActionPointAspect Internal => GetSpecificAspect<ActionPointAspect>();

        public int ActionPoints
        {
            get => Internal.ActionPoints;
            set
            {
                SetProperty(Internal.ActionPoints, value, Internal, 
                    (model, prop) => model.ActionPoints = prop);
                UpdateCost();
            }
        }
    }
}
