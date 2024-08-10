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
        public override bool IsPositive => false;

        /// <summary>
        /// Важный волшебный коэффициент, от которого напрямую зависит динамика боя.
        /// Чем выше коэффициент, тем дороже персонажу обходится его DPR и тем дольше 
        /// будут длиться столкновения с противниками.
        /// </summary>
        private const int _battleTimeCoef = 5;

        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect), 
            typeof(VampirismAspect), typeof(DurationAspect)
        ];

        public override int BaseExpCost()
        {
            double preciseAverageWithDuration = Impact.PreciseAverage() 
                * (GetAspect<DurationAspect>().Duration + 1);

            return (preciseAverageWithDuration * preciseAverageWithDuration * _battleTimeCoef).Round();
        }

        public override int GetExpCost()
        {
            // Исключаем влияние аспекта длительности, так как он уже учтён в BaseExpCost
            DurationAspect durationAspect = GetAspect<DurationAspect>();
            double durationAspectCoef = durationAspect.GetCoefficient();
            double cost = base.GetExpCost() / durationAspectCoef;

            // Переопределяем расчёт длительности в эффекте
            // Чем сильнее растянут урон по времени, тем дешевле
            cost *= Math.Max(0.2, 1 - durationAspect.Duration * 0.1); 

            return cost.Round();
        }

        protected override void InitializeAspect(AspectBase? aspect)
        {
            base.InitializeAspect(aspect);

            if(aspect is DurationAspect durationAspect)
            {
                durationAspect.Duration = 0;
            }
        }
    }

    public static class LikeDamageEffectExtension
    {
        public static int CostLikeDamageEffect(this DicePool impact)
        {
            return new DamageEffect() { Impact = impact }.BaseExpCost();
        }
    }
}
