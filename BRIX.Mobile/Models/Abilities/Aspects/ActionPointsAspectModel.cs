using BRIX.Library.Ability;
using BRIX.Mobile.ViewModel.Abilities;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public partial class ActionPointsAspectModel : SpecificAspectModelBase<ActionPointAbilityAspect>
    {
        public ActionPointsAspectModel(ActionPointAbilityAspect model) : base(model) { }

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
