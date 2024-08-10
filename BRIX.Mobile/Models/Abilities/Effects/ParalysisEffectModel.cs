using BRIX.Library.Effects;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public class ParalysisEffectModel(ParalysisEffect effect) : EffectGenericModelBase<ParalysisEffect>(effect)
    {
        public ParalysisEffectModel() : this(new ParalysisEffect()) { }

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
