using BRIX.Library.Mathematics;

namespace BRIX.Library.Abilities
{
    public class AbilityActivation
    {
        public EActivationStrategy Strategy { get; set; } = EActivationStrategy.ActionPoints;

        private readonly int _minActionPoints = 1;
        private readonly int _maxActionPoints = 50;
        private int _actionPoints = 1;
        public int ActionPoints
        {
            get => _actionPoints;
            set
            {
                if (value < _minActionPoints || _maxActionPoints < value)
                {
                    throw new ArgumentException("Способность может тратить от 1 до 50 очков действий.");
                }

                _actionPoints = value;
            }
        }

        public bool NoCooldown => UsesCount == 0;

        public ECooldownOption Cooldown { get; set; } = ECooldownOption.NoneCooldown;

        /// <summary>
        /// В данном определении соответствие проводится не с коэффициентами, а с отношением временного периода и 
        /// раунда, например в минуте 12 раундов (60 сек / 5 сек), поэтому минуте соответствует множитель 12.
        /// </summary>
        public static Dictionary<ECooldownOption, int> CooldownToPercent => new()
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
        public int UsesCount { get; set; }

        public List<(ETriggerProbability Probability, string Comment)> Triggers { get; set; } = [];

        private double GetActionPointsCoefficient()
        {
            int percent = new ThrasholdCostConverter((1, 0), (2, 20), (3, 10), (6, 1))
                .Convert(ActionPoints);
            double coef = (-percent).ToCoeficient();

            return coef;
        }

        private double GetCooldownCoefficient()
        {
            if (NoCooldown)
            {
                return 1;
            }
            else
            {
                return (CooldownToPercent[Cooldown] / UsesCount).ToCoeficient();
            }
        }

        private double GetTriggersCoefficient()
        {
            return Triggers.Count == 0 ? 1 : Triggers.Sum(x => GetTriggerCoefficient(x.Probability));
        }

        private static double GetTriggerCoefficient(ETriggerProbability trigger)
        {
            return trigger switch
            {
                ETriggerProbability.Low => 2.5,
                ETriggerProbability.Standart => 5,
                ETriggerProbability.High => 10,
                _ => throw new NotImplementedException($"Неизвестная редкость триггера способности {trigger}")
            };
        }

        public double GetCoeficient()
        {
            return Strategy switch
            {
                EActivationStrategy.ActionPoints => 
                    GetActionPointsCoefficient() * GetCooldownCoefficient() * GetTriggersCoefficient(),
                EActivationStrategy.Passive => 10,
                _ => throw new NotImplementedException($"Неизвестная стратегия активации способности {Strategy}"),
            };
        }
    }

    public enum EActivationStrategy
    {
        ActionPoints = 0,
        Passive = 1
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

    /// <summary>
    /// Редкость, с которой
    /// </summary>
    public enum ETriggerProbability
    {
        Low = 0,
        Standart = 1,
        High = 2
    }
}