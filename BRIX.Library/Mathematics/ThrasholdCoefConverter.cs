using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Mathematics
{
    /// <summary>
    /// Класс позволяющий конвертировать значения показателя в очки опыта с прогрессивной
    /// шкалой стоимости за единицу. Например, перевод расстояния в стоимость за метр может
    /// меняться в соответсвии со значением расстояниея, где до 20 метров каждый метр будет
    /// стоить 5 очков, а начиная с 21 и до 100 метров каждый метр будет стоить уже 2 очка.
    /// </summary>
    public class ThrasholdCoefConverter
    {
        public ThrasholdCoefConverter(params (int, int)[] steps)
        {
            AddSteps(steps);
        }

        public LinkedList<ThresholdStep> Steps { get; } = new();

        public int Convert(int value)
        {
            int converted = 0;
            int valueToProcess = value;

            for (LinkedListNode<ThresholdStep> node = Steps.First; node != null; node = node.Next)
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

        private void AddSteps((int, int)[] steps)
        {
            steps = steps.OrderBy(x => x.Item1).ToArray();

            LinkedListNode<ThresholdStep> current = new(
                new ThresholdStep
                {
                    Thrashold = steps.First().Item1,
                    Delta = steps.First().Item2
                }
            );

            Steps.AddFirst(current);

            foreach ((int, int) step in steps.Skip(1))
            {
                LinkedListNode<ThresholdStep> next = new(
                    new ThresholdStep
                    {
                        Thrashold = step.Item1,
                        Delta = step.Item2
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
