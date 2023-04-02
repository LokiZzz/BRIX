using BRIX.Library.Extensions;

namespace BRIX.Library.DiceValue
{
    public static class DiceValueExtensions
    {
        public static int Average(this Dice dice)
        {
            double n = dice.NumberOfFaces;
            double average = (double)(1 + n) / 2;

            return (average * dice.Count).Round();
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

        public static bool IsValidDicePool(this string input)
        {
            input = input.Replace(" ", string.Empty);
            string[] splittedInput = input.Split('+', StringSplitOptions.RemoveEmptyEntries);

            return splittedInput.All(x =>
                DicePool.DiceRegex.IsMatch(x) 
                || DicePool.MultiDiceRegex.IsMatch(x)
                || DicePool.ModRegex.IsMatch(x)
            );
        }
    }
}
