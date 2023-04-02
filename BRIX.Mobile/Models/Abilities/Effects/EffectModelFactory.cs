using BRIX.Library.Effects;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public static class EffectModelFactory
    {
        public static EffectModelBase GetModel(EffectBase effect)
        {
            switch(effect)
            {
                case DamageEffect damage:
                    return new DamageEffectModel(damage);
            }

            return null;
        }
    }
}
