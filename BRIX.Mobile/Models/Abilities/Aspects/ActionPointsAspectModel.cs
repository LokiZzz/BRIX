using BRIX.Library.Aspects;

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

        public override string Description => $"Ability spends {Internal.ActionPoints} action points.";
    }
}
