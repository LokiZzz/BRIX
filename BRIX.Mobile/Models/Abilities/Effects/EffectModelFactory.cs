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
                case CleanseEffect cle:
                    return new CleanseEffectModel(cle);
                case CancelationEffect can:
                    return new CancelationEffectModel(can);
                case MoveTargetEffect mte:
                    return new MoveTargetEffectModel(mte);
                case MoveCharacterEffect mce:
                    return new MoveCharacterEffectModel(mce);
                case MoveAreaEffect mae:
                    return new MoveAreaEffectModel(mae);
                default:
                {
                    MethodInfo? method = typeof(EffectModelFactory).GetMethod(
                        nameof(GetDefaultModel),
                        BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                    );
                    MethodInfo? genericMethod = method?.MakeGenericMethod(effect.GetType());

                    return genericMethod?.Invoke(null, [effect]) as EffectModelBase
                        ?? throw new Exception($"Модель эффекта не найдена для {effect.GetType()}");
                }
            }
        }

        private static EffectGenericModelBase<T> GetDefaultModel<T>(T effect) where T : EffectBase, new()
        {
            return new EffectGenericModelBase<T>(effect);
        }
    }
}
