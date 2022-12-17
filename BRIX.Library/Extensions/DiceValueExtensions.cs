using BRIX.Library.Extensions;
using BRIX.Utility.Extensions;

namespace BRIX.Library.DiceValue
{
    public static class DiceValueExtensions
    {
        public static int Average(this Dice dice)
        {
            double n = dice.NumberOfFaces;
            double average = (1 + n) / 2;

            return average.Round() * dice.Count;
        }

        public static int Average(this DicePool dicePool)
        {
            return dicePool.Dice.Sum(x => x.Average()) + dicePool.Modifier; 
        }

        public static int Min(this DicePool dicePool)
        {
            return dicePool.Dice.Sum(x => x.Count) + dicePool.Modifier;
        }

        public static int Max(this DicePool dicePool)
        {
            return dicePool.Dice.Sum(x => x.NumberOfFaces * x.Count) + dicePool.Modifier;
        }
    }
}
