using BRIX.Library.Effects;
using System.Reflection;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public static class EffectModelFactory
    {
        public static EffectModelBase GetModel(EffectBase effect)
        {
            switch(effect)
            {
                // Здесь добавляются варианты только для тех эффектов, у которых реализована своя модель,
                // то есть для те, которые не используют EffectGenericModelBase<T> напрямую, а наследуются от него.
                //case DamageEffect dmg:
                //    return new DamageEffectModel(dmg);
                default:
                {
                    MethodInfo method = typeof(EffectModelFactory).GetMethod(
                        nameof(GetDefaultModel),
                        BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                    );
                    MethodInfo genericMethod = method.MakeGenericMethod(effect.GetType());
                    
                    return genericMethod.Invoke(null, new object[] { effect }) as EffectModelBase;
                }
            }
        }

        private static EffectGenericModelBase<T> GetDefaultModel<T>(T effect) where T : EffectBase, new()
        {
            return new EffectGenericModelBase<T>(effect);
        }
    }
}
