using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Урон.
    /// </summary>
    public class DamageEffect : DiceImpactEffectBase
    {
        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect), typeof(VampirismAspect)
        ];

        public override int GetExpCost()
        {
            TargetSelectionAspect? targetSelection = GetAspect<TargetSelectionAspect>();

            // Если способность наносит урон самому персонажу, то все остальные аспекты игнорируются.
            // Эффект в таком случае не добавляет к стоимости, а уменьшает её.
            if (targetSelection?.Strategy == ETargetSelectionStrategy.CharacterHimself)
            {
                return -BaseExpCost();
            }

            // Иначе рассчёт происходит как обычно.
            return base.GetExpCost();
        }

        public override int BaseExpCost() => Impact.Average() * Impact.Average();
    }
}
