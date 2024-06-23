using BRIX.Library.Aspects;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Создаёт область, наносящую урон персонажам, которые начали в ней ход или переместились в ней.
    /// Урон наносится за каждый метр, на который персонаж переместился в области.
    /// </summary>
    public class DangerousTerrainEffect : DiceImpactEffectBase
    {
        public override List<Type> RequiredAspects => 
        [
            typeof(AOEAspect), typeof(DurationAspect), 
            typeof(ActivationConditionsAspect), typeof(VampirismAspect)
        ];

        public override int BaseExpCost() => Impact.Average() * Impact.Average();
    }
}
