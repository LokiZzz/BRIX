using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Защита. Уменьшение входящего урона.
    /// </summary>
    public class DefenseEffect : DiceImpactEffectBase
    {
        public override bool HasStatus => true;

        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect),
            typeof(DurationAspect),
        ];

        public override int BaseExpCost()
        {
            // Защищаться должно быть более выгодно, чем наносить урон,
            // иначе будет возможен эффект непробиваемого танка и динамика боёв сильно снизится
            // из-за этой меты.
            return (Impact.Average() * Impact.Average() * 1.1).Round();
        }
    }
}
