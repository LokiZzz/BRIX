using BRIX.Library.Effects;

namespace BRIX.Web.Client.Models.Abilities
{
    public static class EffectsDictionary
    {
        public static List<EffectDictionaryVM> Collection => 
        [
            new (typeof(DamageEffect), "dmg"),
            new (typeof(HealEffect), "heal"),
            new (typeof(SummonCreatureEffect), "smn"),
        ];

        public static EffectBase Create(this Type effectType)
        {
            if (effectType.IsAssignableFrom(typeof(EffectBase)))
            {
                throw new Exception($"Type {effectType.GetType()} is not an ability effect.");
            }

            return Activator.CreateInstance(effectType) as EffectBase
                ?? throw new Exception("Create effect from item failed.");
        }

        public static string GetLocalizedName(this Type type) => $"Effect_{type.Name}";

        public static string GetRoute(EffectBase effect)
        {
            EffectDictionaryVM effectVM = Collection.Single(x => x.Type.Equals(effect.GetType()));

            return effectVM.Route;
        }
    }

    public class EffectDictionaryVM(Type type, string route)
    {
        public Type Type { get; set; } = type;

        public string Route { get; set; } = route;
    }
}
