﻿namespace BRIX.Library.DiceValue
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
            double mathExpectation = 0;
            
            // M(x)=Σpx
            GetAverageRecursive(
                rerollValues, 
                explodingDepth, 
                currentExplodingDepth: 0,
                ref mathExpectation
            );

            return mathExpectation * Count;
        }

        private void GetAverageRecursive(
            List<int>? rerollValues, 
            int explodingDepth,
            int currentExplodingDepth,
            ref double mathExpectation)
        {
            List<int> validFaces = Enumerable.Range(1, NumberOfFaces)
                .Where(x => rerollValues?.Any(y => y == x) != true)
                .ToList();

            foreach (int i in validFaces)
            {
                if(i != NumberOfFaces || currentExplodingDepth == explodingDepth)
                {
                    double probability = currentExplodingDepth == 0
                        ? 1d / validFaces.Count
                        : 1d / Math.Pow(validFaces.Count, currentExplodingDepth + 1);
                    mathExpectation += (i + NumberOfFaces * currentExplodingDepth) * probability;
                }
                else
                {
                    currentExplodingDepth++;
                    GetAverageRecursive(rerollValues, explodingDepth, currentExplodingDepth, ref mathExpectation);
                }
            }
        }

        public int Min(List<int>? rerollValues)
        {
            int lowestValue = Enumerable.Range(1, NumberOfFaces)
                .Where(x => rerollValues?.Any(y => y == x) != true)
                .Min();

            return lowestValue * Count;
        }

        public int Max(List<int>? rerollValues, int explodingDepth)
        {
            int highestValue = Enumerable.Range(1, NumberOfFaces)
                .Where(x => rerollValues?.Any(y => y == x) != true)
                .Max();

            return highestValue == NumberOfFaces
                ? highestValue * (explodingDepth + 1) * Count
                : highestValue * Count;
        }
    }
}
