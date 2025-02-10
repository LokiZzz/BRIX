using BRIX.Library.Effects;

Console.WriteLine("Hello, World!");

EffectsCollectionItem item = new (typeof(DamageEffect), "");

public class EffectsCollectionItem
{
    public EffectsCollectionItem(Type effectType, string route)
    {
        if (effectType.IsAssignableTo(typeof(EffectBase)))
        {
            throw new Exception($"Type {effectType.GetType()} is not an ability effect.");
        }

        Type = effectType;
        LocalizedNameKey = $"Effect_{effectType.Name}";
    }

    public Type Type { get; set; }

    public string LocalizedNameKey { get; set; } = string.Empty;

    public string Route { get; set; } = string.Empty;
}