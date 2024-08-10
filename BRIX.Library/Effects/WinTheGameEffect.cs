namespace BRIX.Library.Effects
{
    public class WinTheGameEffect : EffectBase
    {
        public override bool IsPositive => true;
        
        public override List<Type> RequiredAspects => [];

        public override int BaseExpCost() => 777;
    }
}
