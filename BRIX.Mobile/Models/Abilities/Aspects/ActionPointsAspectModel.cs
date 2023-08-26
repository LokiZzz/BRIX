using BRIX.Library.Aspects;
using BRIX.Mobile.ViewModel.Abilities;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public partial class ActionPointsAspectModel : SpecificAspectModelBase<ActionPointAspect>
    {
        public ActionPointsAspectModel(ActionPointAspect model) : base(model) { }

        public int ActionPoints
        {
            get => Internal.ActionPoints;
            set
            {
                SetProperty(Internal.ActionPoints, value, Internal, 
                    (model, prop) => model.ActionPoints = prop);
            }
        }
    }
}
