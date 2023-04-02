using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    public class HealEffect : DamageEffect
    {
        public override int BaseExpCost() => (base.BaseExpCost() * 1.2).Round();
    }
}
