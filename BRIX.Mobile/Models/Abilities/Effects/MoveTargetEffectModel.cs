using BRIX.Library.Effects;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public class MoveTargetEffectModel(MoveTargetEffect effect) : EffectGenericModelBase<MoveTargetEffect>(effect)
    {
        public MoveTargetEffectModel() : this(new MoveTargetEffect()) { }

        public int DistanceInMeters
        {
            get => Internal.DistanceInMeters;
            set
            {
                SetEffectProperty(Internal.DistanceInMeters, value, Internal, (model, prop) => {
                    model.DistanceInMeters = prop;
                });
            }
        }

        public EMoveTargetPath Path
        {
            get => Internal.TargetPath;
            set
            {
                SetEffectProperty(Internal.TargetPath, value, Internal, (model, prop) => {
                    model.TargetPath = prop;
                });
            }
        }
    }
}
