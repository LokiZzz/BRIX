using BRIX.Library.Mathematics;

namespace BRIX.Library.Abilities
{
    public class AbilityActivation
    {
        private readonly int _minActionPoints = 1;
        private readonly int _maxActionPoints = 25;

        private int _actionPoints = 3;
        public int ActionPoints
        {
            get => _actionPoints;
            set
            {
                if (value < _minActionPoints || _maxActionPoints < value)
                {
                    throw new ArgumentException("Способность может тратить от 1 до 25 очков действий.");
                }

                _actionPoints = value;
            }
        }


        /// <summary>
        /// Количество раз в день, которое можно использовать способность до того, как ей понадобится перезарядка.
        /// </summary>
        public int UsesCountPerDay { get; set; }

        public bool HasCooldown => UsesCountPerDay != 0;

        public bool NoCooldown => UsesCountPerDay == 0;

        public List<(ETriggerProbability Probability, string Comment)> Triggers { get; set; } = [];

        /// <summary>
        /// Применить настройки активации к расчёту стоимости способности.
        /// </summary>
        /// <param name="exp">Стоимость способности без учёта настроек активации.</param>
        /// <returns>Результирующая стоимость с учётом настроек активации без округления.</returns>
        public double Apply(int exp)
        {
            double resultCost = exp;

            if (Triggers.Count == 0)
            {
                resultCost *= ActionPoints switch
                {
                    1 => 14, // sqrt(3/1) = (9 -> 14)
                    2 => 3, // (2.25 -> 3)
                    3 => 1, // base
                    4 => 0.7, // sqrt(3/4) = (0.5625 -> 0.7)
                    5 => 0.5, // (0.36 -> 0.5)
                    >= 6 and <= 25 => GetActionPointCoeficient(),
                    _ => 0.05
                };

            }

            if (HasCooldown)
            {
                resultCost *= (-25 / UsesCountPerDay).ToCoeficient();
            }

            if (Triggers.Count > 0)
            {
                resultCost *= Triggers.Sum(x => GetTriggerCoefficient(x.Probability));
            }

            return resultCost;
        }

        // После 5 увеличение кол-ва очков действий даёт следующие скидки:
        // 6-10 по -3%; 11-15 по -2%; 16-50 по -1% за каждое очко действий.
        private double GetActionPointCoeficient()
        {
            return (-new ThrasholdCostConverter((1, 0), (5, 64), (6, 3), (11, 2), (16, 1))
                .Convert(ActionPoints))
                .ToCoeficient();
        }

        private static double GetTriggerCoefficient(ETriggerProbability trigger)
        {
            return trigger switch
            {
                ETriggerProbability.Low => 2.5,
                ETriggerProbability.Medium => 5,
                ETriggerProbability.High => 10,
                _ => throw new NotImplementedException($"Неизвестная редкость триггера способности {trigger}")
            };
        }
    }

    /// <summary>
    /// Вероятность возникновения триггера.
    /// </summary>
    public enum ETriggerProbability
    {
        Low = 0,
        Medium = 1,
        High = 2
    }
}