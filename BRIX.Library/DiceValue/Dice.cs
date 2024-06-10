using BRIX.Library.Extensions;

namespace BRIX.Library.DiceValue
{
    public class Dice
    {
        public Dice() { }

        public Dice(int numberOfFaces, int count = 1)
        {
            NumberOfFaces = numberOfFaces;
            Count = count;
        }

        public int NumberOfFaces { get; set; }

        public int Count { get; set; }

        /// <summary>
        /// Расчёт математического ожидания с учётом взрыва и перебросов.
        /// </summary>
        public double Average(List<int>? rerollValues = null, int explodingDepth = 0)
        {
            List<int>? rerollThis = rerollValues?
                .Where(x => x > 0 && x < NumberOfFaces)
                .ToList();

            double mathExpectation = 0;
            
            // M(x)=Σpx
            GetAverageRecursive(
                rerollThis, 
                explodingDepth, 
                currentExplodingDepth: 0,
                ref mathExpectation
            );

            return mathExpectation;
        }

        private void GetAverageRecursive(
            List<int>? rerollThis, 
            int explodingDepth,
            int currentExplodingDepth,
            ref double mathExpectation)
        {
            foreach (int i in Enumerable.Range(1, NumberOfFaces))
            {
                if (rerollThis?.Any(x => x == i) == true)
                {
                    continue;
                }

                if(i != NumberOfFaces)
                {
                    double probability = currentExplodingDepth == 0
                        ? 1d / NumberOfFaces
                        : 1d / Math.Pow(NumberOfFaces, currentExplodingDepth + 1);
                    mathExpectation += (i + NumberOfFaces * currentExplodingDepth) * probability;
                }
                else
                {
                    if(currentExplodingDepth == explodingDepth)
                    {
                        double probability = 1d / Math.Pow(NumberOfFaces, currentExplodingDepth + 1);
                        mathExpectation += (i + NumberOfFaces * currentExplodingDepth) * probability;
                    }

                    if(currentExplodingDepth < explodingDepth)
                    {
                        currentExplodingDepth++;
                        GetAverageRecursive(rerollThis, explodingDepth, currentExplodingDepth, ref mathExpectation);
                    }
                }
            }
        }
    }
}
