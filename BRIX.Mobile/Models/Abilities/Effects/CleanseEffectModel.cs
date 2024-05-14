using BRIX.Library.Effects;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public class CleanseEffectModel(CleanseEffect effect) : EffectGenericModelBase<CleanseEffect>(effect)
    {
        public CleanseEffectModel() : this(new CleanseEffect()) { }

        public int MaxStatusPower
        {
            get => Internal.MaxStatusPower;
            set
            {
                SetEffectProperty(Internal.MaxStatusPower, value, Internal, (model, prop) => {
                    model.MaxStatusPower = prop;
                });
            }
        }

        public int StatusesCount
        {
            get => Internal.StatusesCount;
            set
            {
                SetEffectProperty(Internal.StatusesCount, value, Internal, (model, prop) => {
                    model.StatusesCount = prop;
                });
            }
        }
    }
}
