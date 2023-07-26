using BRIX.Library.Effects;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public static class EffectModelFactory
    {
        public static EffectModelBase GetModel(EffectBase effect)
        {
            switch(effect)
            {
                case HealEffect heal:
                    return new HealEffectModel(heal);
                case DamageEffect dmg:
                    return new DamageEffectModel(dmg);
            }

            return null;
        }
    }
}
