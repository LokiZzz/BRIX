namespace BRIX.Library.Effects
{
    public class WinTheGameEffect : EffectBase
    {
        public override List<Type> RequiredAspects => new ();

        public override int BaseExpCost() => 777;
    }
}
