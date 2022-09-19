namespace BRIX.Library.Aspects
{
    public class CooldownAspect : SingleConditionAspect<ECooldownOption>
    {
        public override Dictionary<ECooldownOption, int> ConditionToCoeficientMap => new Dictionary<ECooldownOption, int>
        {
            { ECooldownOption.None, 0 },
            { ECooldownOption.Minute, -10 },
            { ECooldownOption.Hour, -15 },
            { ECooldownOption.Day, -20 },
            { ECooldownOption.Week, -25 },
            { ECooldownOption.Month, -30 },
            { ECooldownOption.Year, -35 },
            { ECooldownOption.TenYears, -40 },
            { ECooldownOption.HundredYears, -45 },
            { ECooldownOption.CannotReset, -70 }
        };
    }

    public enum ECooldownOption
    {
        None = 0,
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
