namespace BRIX.Library.Effects
{
    public class WinTheGameEffect : EffectBase
    {
        public override List<Type> RequiredAspects => new ();

        public override int BaseExpCost() => 777;
    }

    public class DummyEffect : EffectBase
    {
        public override List<Type> RequiredAspects => new();

        public override int BaseExpCost() => 10000;
    }
}
