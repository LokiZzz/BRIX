using BRIX.Library.Effects;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public class MutenessEffectModel(MutenessEffect effect) : EffectGenericModelBase<MutenessEffect>(effect)
    {
        public MutenessEffectModel() : this(new MutenessEffect()) { }

        public int HealthThreshold
        {
            get => Internal.HealthThreshold;
            set
            {
                SetEffectProperty(Internal.HealthThreshold, value, Internal, (model, prop) => {
                    model.HealthThreshold = prop;
                });
            }
        }
    }
}
