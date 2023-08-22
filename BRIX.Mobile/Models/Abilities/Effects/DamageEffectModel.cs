using BRIX.Library.DiceValue;
using BRIX.Library.Effects;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    /// <summary>
    /// Эта модель не несёт в себе никакой смысловой нагрузки и лишь служит напоминанием о том, что если модель эффекта
    /// должна хранить какое-то особое состояние или реализовывать поведение, то можно наследоваться от 
    /// EffectGenericModelBase<T>
    /// </summary>
    public partial class DamageEffectModel : EffectGenericModelBase<DamageEffect> 
    {
        public DamageEffectModel() { }
        public DamageEffectModel(DamageEffect effect) : base(effect) { }
    }
}
