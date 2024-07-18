using BRIX.Library.Aspects;
using BRIX.Library.Extensions;

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

        public bool IsAreaDisposable { get; set; }

        public override int BaseExpCost()
        {
            // Способ наносить урон именно этим способом может быть более или менее эффективным, чем прямой метод.
            // В зависимости от реального положения дел во время плейтеста эта пара коэффициентов могут меняться.
            double areaMethodCoefficient = IsAreaDisposable ? 0.3 : 0.9;

            return (Impact.CostLikeDamageEffect() * areaMethodCoefficient).Round();
        }
    }
}
