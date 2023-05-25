namespace BRIX.Library.Aspects
{
    public class CooldownAspect : SingleConditionAspect<ECooldownOption>
    {
        public override Dictionary<ECooldownOption, int> ConditionToCoeficientMap => new Dictionary<ECooldownOption, int>
        {
            { ECooldownOption.NoneCooldown, 0 },
            { ECooldownOption.Minute, -20 },
            { ECooldownOption.Hour, -30 },
            { ECooldownOption.Day, -40 },
            { ECooldownOption.Week, -50 },
            { ECooldownOption.Month, -60 },
            { ECooldownOption.Year, -70 },
            { ECooldownOption.TenYears, -80 },
            { ECooldownOption.HundredYears, -90 },
            { ECooldownOption.CannotReset, -95 }
        };

        /// <summary>
        /// Количество раз, которое можно использовать способность до того, как ей понадобится перезарядка.
        /// 0 означает, что способность можно использовать любое количество раз без ограничений.
        /// </summary>
        public int UsesCount { get; set; } = 0;

        public override double GetCoefficient() => base.GetCoefficient() / UsesCount;
    }

    public enum ECooldownOption
    {
        NoneCooldown = 0,
        Minute = 10,
        Hour = 15,
        Day = 20,
        Week = 25,
        Month = 30,
        Year = 35,
        TenYears = 40,
        HundredYears = 45,
        CannotReset = 70
    }
}
