using BRIX.Library.Aspects;
using BRIX.Library.Extensions;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Перемещение области действия способности.
    /// </summary>
    public class MoveAreaEffect : EffectBase
    {
        public override bool IsPositive => false;

        public override List<Type> RequiredAspects =>
        [
            typeof(ActivationConditionsAspect)
        ];

        /// <summary>
        /// Максимальное расстояние, на которое будет перемещена область.
        /// </summary>
        public int MovingDistance { get; set; } = 1;

        /// <summary>
        /// Максимальное расстояние от персонажа до перемещаемой области в метрах.
        /// </summary>
        public int DistanceToArea { get; set; } = 1;

        /// <summary>
        /// Максимальное количество перемещаемых областей.
        /// </summary>
        public int MaxAreaCount { get; set; } = 1;

        /// <summary>
        /// Масимальная мощность (стоимость) способности, которая создала область.
        /// </summary>
        public int MaxAbilityPower { get; set; } = 100;

        /// <summary>
        /// Можно ли перемещать любые области, или только от своих способностей.
        /// </summary>
        public bool OnlyYourAbilities { get; set; } = true;

        public override int BaseExpCost()
        {
            double distanceToAreaCoef = new ThrasholdCostConverter((1, 0), (2, 10), (21, 5))
                .Convert(DistanceToArea)
                .ToCoeficient();
            double movingDistanceCoef = new ThrasholdCostConverter((1, 0), (2, 10), (21, 5))
                .Convert(MovingDistance)
                .ToCoeficient();
            double onlyYourAbilitiesCoef = OnlyYourAbilities ? 1 : 2;

            return (MaxAbilityPower 
                * MaxAreaCount 
                * 0.5 
                * distanceToAreaCoef
                * movingDistanceCoef
                * onlyYourAbilitiesCoef)
                .Round();
        }
    }
}
