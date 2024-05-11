using BRIX.Library.DiceValue;

namespace BRIX.Library.Effects
{
    public abstract class DiceImpactEffectBase : EffectBase
    {
        public DicePool Impact { get; set; } = new DicePool();
    }
}
