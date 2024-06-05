using BRIX.Library.Effects;
namespace BRIX.Mobile.Models.Abilities.Effects
{
    public class ShieldEffectModel(ShieldEffect effect) : EffectGenericModelBase<ShieldEffect>(effect)
    {
        public ShieldEffectModel() : this(new ShieldEffect()) { }

        public int Durability
        {
            get => Internal.Durability;
            set
            {
                SetEffectProperty(Internal.Durability, value, Internal, (model, prop) => {
                    model.Durability = prop;
                });
            }
        }

        public bool CanBePermeable
        {
            get => Internal.CanBePermeable;
            set
            {
                SetEffectProperty(Internal.CanBePermeable, value, Internal, (model, prop) => {
                    model.CanBePermeable = prop;
                });
            }
        }

        public bool IsTransparent
        {
            get => Internal.IsTransparent;
            set
            {
                SetEffectProperty(Internal.IsTransparent, value, Internal, (model, prop) => {
                    model.IsTransparent = prop;
                });
            }
        }
    }
}
