using BRIX.Library.Aspects;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Ability
{
    public class AbilityActivation
    {
        public EActivationStrategy Strategy { get; set; } = EActivationStrategy.ActionPoints;

        private int _minActionPoints = 1;
        private int _maxActionPoints = 50;
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
        public Dictionary<ECooldownOption, int> CooldownToCoeficient => new Dictionary<ECooldownOption, int>
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

        public ETriggerPropbability TriggerProbability { get; set; }

        private double GetActionPointsCoefficient()
        {
            int percent = new ThrasholdCostConverter((1, 0), (2, 20), (3, 10), (6, 1))
                .Convert(ActionPoints);
            double coef = (-percent).ToCoeficient();

            return coef;
        }

        private double GetCooldownCoefficient()
        {
            if (UsesCount == 0)
            {
                return CooldownToCoeficient[Cooldown];
            }
            else
            {
                return (CooldownToCoeficient[Cooldown] / UsesCount).ToCoeficient();
            }
        }

        private double GetTriggerCoefficient()
        {
            return TriggerProbability switch
            {
                ETriggerPropbability.Low => 2,
                ETriggerPropbability.Medium => 5,
                ETriggerPropbability.High => 10,
                _ => throw new NotImplementedException($"Неизвестная редкость триггера способности {TriggerProbability}")
            };
        }

        public double GetCoeficient()
        {
            switch (Strategy)
            {
                case EActivationStrategy.ActionPoints:
                    return GetActionPointsCoefficient() * GetCooldownCoefficient();
                case EActivationStrategy.Trigger:
                    return GetTriggerCoefficient() * GetCooldownCoefficient();
                case EActivationStrategy.Passive:
                    return 10;
                default:
                    throw new NotImplementedException($"Неизвестная стратегия активации способности {Strategy}");
            }
        }
    }

    public enum EActivationStrategy
    {
        ActionPoints = 0,
        Trigger = 1,
        Passive = 2
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
    public enum ETriggerPropbability
    {
        Low = 0,
        Medium = 1,
        High = 2
    }
}