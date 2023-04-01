using BRIX.Library.Aspects;
using BRIX.Library.DiceValue;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public partial class ActionPointsAspectModel : AspectModelBase
    {
        public ActionPointsAspectModel() : this(new ActionPointAspect()) { }

        public ActionPointsAspectModel(ActionPointAspect model)
        {
            InternalModel = model;
        }

        public ActionPointAspect Internal => GetSpecificAspect<ActionPointAspect>();
    }
}
