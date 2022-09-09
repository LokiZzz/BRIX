using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Mathematics
{
    
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
