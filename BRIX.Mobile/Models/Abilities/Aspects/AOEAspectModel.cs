using BRIX.Library.Aspects;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public partial class AOEAspectModel : SpecificAspectModelBase<AOEAspect>
    {
        public AOEAspectModel(AOEAspect model) : base(model) 
        {
            AreaShape = new VolumeShapeModel(Internal.AreaShape);
        }

        public VolumeShapeModel AreaShape { get; set; }

        public int AreaDistance
        {
            get => Internal.DistanceToArea;
            set
            {
                SetProperty(Internal.DistanceToArea, value, Internal, 
                    (model, prop) => model.DistanceToArea = prop
                );
            }
        }

        public bool AreaCanBeBounded
        {
            get => Internal.CanBeBounded;
            set
            {
                SetProperty(Internal.CanBeBounded, value, Internal,
                    (model, prop) => model.CanBeBounded = prop
                );
            }
        }
    }
}