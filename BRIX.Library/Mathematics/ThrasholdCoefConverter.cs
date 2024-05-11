namespace BRIX.Library.Mathematics
{
    /// <summary>
    /// Калькулятор для прогрессивных шкал.
    /// </summary>
    public class ThrasholdCostConverter
    {
        /// <summary>
        /// Инициализируется шагами прогрессивной шкалы. Каждому значению соответствует  некоторая стоимость в очках.
        /// Это стоимость может менятся порогами, например каждая единица значения от 1 до 5 стоит 100 очков, 
        /// а каждая единица от 6 и более стоит 50 очков. Тогда значение 5 будет стоить 500 очков, а значение 6 — 550.
        /// Например, калькулятор инициализирован такой шкалой: (1, 100), (6, 50), (11, 25).
        /// Тогда если метод Convert(х) калькулятора — это функция f, 
        /// а аргумент функции  — это значение для конверсии, то:
        /// x:      1   2   3   4   5   6   7   8   9   10  11  12 
        /// f(x):   100 200 300 400 500 550 600 650 700 750 775 800
        /// </summary>
        public ThrasholdCostConverter(params (int stepFrom, int percentIncrease)[] steps)
        {
            AddSteps(steps);
        }

        public LinkedList<ThresholdStep> Steps { get; } = new();

        public int Convert(int value)
        {
            int converted = 0;
            int valueToProcess = value;

            for (LinkedListNode<ThresholdStep>? node = Steps.First; node != null; node = node.Next)
            {
                ThresholdStep current = node.Value;
                ThresholdStep? next = node.Next?.Value;

                if(value > current.Thrashold)
                {
                    if (next != null && value >= next.Thrashold)
                    {
                        converted += (next.Thrashold - current.Thrashold) * current.Delta;
                        valueToProcess -= next.Thrashold - current.Thrashold;
                    }
                    else
                    {
                        converted += valueToProcess * current.Delta;

                        break;
                    }
                }
                else
                {
                    converted += valueToProcess * current.Delta;

                    break;
                }
            }

            return converted;
        }

        private void AddSteps((int Thrashold, int Delta)[] steps)
        {
            steps = steps.OrderBy(x => x.Item1).ToArray();

            LinkedListNode<ThresholdStep> current = new(
                new ThresholdStep
                {
                    Thrashold = steps.First().Thrashold,
                    Delta = steps.First().Delta
                }
            );

            Steps.AddFirst(current);

            foreach ((int Thrashold, int Delta) step in steps.Skip(1))
            {
                LinkedListNode<ThresholdStep> next = new(
                    new ThresholdStep
                    {
                        Thrashold = step.Thrashold,
                        Delta = step.Delta
                    }
                );

                Steps.AddAfter(current, next);
                current = next;
            }
        }
    }

    public class ThresholdStep
    {
        public int Thrashold { get; set; }

        public int Delta { get; set; }
    }
}
