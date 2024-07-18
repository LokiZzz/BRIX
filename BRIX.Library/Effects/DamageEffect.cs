using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.DiceValue;
using BRIX.Library.Extensions;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Урон.
    /// </summary>
    public class DamageEffect : DiceImpactEffectBase
    {
        /// <summary>
        /// Важный волшебный коэффициент, от которого напрямую зависит динамика боя.
        /// Чем выше коэффициент, тем дороже персонажу обходится его DPR и тем дольше 
        /// будут длиться столкновения с противниками.
        /// </summary>
        private const int _battleTimeCoef = 5;

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

        public override int BaseExpCost() => 
            (Impact.PreciseAverage() * Impact.PreciseAverage() * _battleTimeCoef).Round();
    }

    public static class LikeDamageEffectExtension
    {
        public static int CostLikeDamageEffect(this DicePool impact)
        {
            return new DamageEffect() { Impact = impact }.BaseExpCost();
        }
    }
}
