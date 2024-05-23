using BRIX.Library.Effects;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public class MoveCharacterEffectModel(MoveCharacterEffect effect) : EffectGenericModelBase<MoveCharacterEffect>(effect)
    {
        public MoveCharacterEffectModel() : this(new MoveCharacterEffect()) { }

        public double DistancePerActionPoint
        {
            get => Internal.DistancePerActionPoint;
            set
            {
                SetEffectProperty(Internal.DistancePerActionPoint, value, Internal, (model, prop) => {
                    model.DistancePerActionPoint = prop;
                });
            }
        }

        public ECharacterMovingType MovingType
        {
            get => Internal.MovingType;
            set
            {
                SetEffectProperty(Internal.MovingType, value, Internal, (model, prop) => {
                    model.MovingType = prop;
                });
            }
        }
    }
}
