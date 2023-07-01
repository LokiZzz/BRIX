using BRIX.Library.Extensions;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects
{
    public class CooldownAspect : SingleConditionAspect<ECooldownOption>
    {
        /// <summary>
        /// В данном определении соответствие проводится не с коэффициентами, а с отношением временного периода и 
        /// раунда, например в минуте 12 раундов (60 сек / 5 сек), поэтому минуте соответствует множитель 12.
        /// </summary>
        public override Dictionary<ECooldownOption, int> ConditionToCoeficientMap => new Dictionary<ECooldownOption, int>
        {
            { ECooldownOption.NoneCooldown, 0 },
            { ECooldownOption.Round,        -10 },
            { ECooldownOption.Minute,       -25 },
            { ECooldownOption.Hour,         -50 },
            { ECooldownOption.Day,          -75 },
            { ECooldownOption.Week,         -80 },
            { ECooldownOption.Month,        -85 },
            { ECooldownOption.Year,         -90 },
            { ECooldownOption.TenYears,     -93 },
            { ECooldownOption.HundredYears, -96 },
            { ECooldownOption.CannotReset,  -99 }
        };

        /// <summary>
        /// Количество раз, которое можно использовать способность до того, как ей понадобится перезарядка.
        /// 0 означает, что способность можно использовать любое количество раз без ограничений.
        /// </summary>
        public int UsesCount { get; set; } = 0;

        public override double GetCoefficient()
        {
            if(UsesCount == 0)
            {
                return base.GetCoefficient();
            }
            else
            {
                return (ConditionToCoeficientMap[Condition] / UsesCount).ToCoeficient();
            }
        }
    }

    public enum ECooldownOption
    {
        NoneCooldown,
        Round,
        Minute,
        Hour,
        Day,
        Week,
        Month,
        Year,
        TenYears,
        HundredYears,
        CannotReset
    }
}
