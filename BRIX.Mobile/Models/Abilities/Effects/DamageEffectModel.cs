using BRIX.Library.DiceValue;
using BRIX.Library.Effects;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DamageEffectModel : EffectGenericModelBase<DamageEffect> 
    {
        public DamageEffectModel() { }
        public DamageEffectModel(DamageEffect effect) : base(effect) { }
    }
}
