using BRIX.Library.Effects;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public class CancelationEffectModel(CancelationEffect effect) : EffectGenericModelBase<CancelationEffect>(effect)
    {
        public CancelationEffectModel() : this(new CancelationEffect()) { }

        public int MaxAbilityPower
        {
            get => Internal.MaxAbilityPower;
            set
            {
                SetEffectProperty(Internal.MaxAbilityPower, value, Internal, (model, prop) => {
                    model.MaxAbilityPower = prop;
                });
            }
        }
    }
}
