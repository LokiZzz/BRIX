using BRIX.Library.Extensions;
using System.Text;
using System.Text.RegularExpressions;

namespace BRIX.Library.DiceValue
{
    public class DicePool
    {
        /// <summary>
        /// Конструктор для набора костей.
        /// </summary>
        /// <param name="modifier">Константа. Будет прибавлена после броска.</param>
        /// <param name="dice">Кости, добавляются в традиционном формате, например 3d6 => (3, 6).</param>
        public DicePool(int modifier, params (int, int)[] dice)
        {
            Modifier = modifier;
            SetDice(dice);
        }

        /// <summary>
        /// Конструктор для набора костей.
        /// </summary>
        /// <param name="dice">Кости, добавляются в традиционном формате, например 3d6 => (3, 6).</param>
        public DicePool(params (int, int)[] dice)
        {
            SetDice(dice);
        }

        public List<Dice> Dice { get; set; } = new List<Dice>();

        public int Modifier { get; set; }

        public bool IsEmpty => !Dice.Any() && Modifier == default;

        /// <summary>
        /// Добавляет в пул копию переданных костей.
        /// </summary>
        /// <param name="dice"></param>
        public void Add(Dice dice)
        {
            Dice.Add(new Dice(dice.NumberOfFaces, dice.Count));
            Normalize();
        }

        private void SetDice((int, int)[] dice)
        {
            Dice.Clear();

            foreach ((int, int) singleDiceType in dice)
            {
                Dice.Add(new Dice(singleDiceType.Item2, singleDiceType.Item1));
            }

            Normalize();
        }

        private void Normalize()
        {
            List<Dice> diceListToSet = new List<Dice>();

            foreach(Dice singleDiceType in Dice)
            {
                Dice? dice = diceListToSet.FirstOrDefault(x => x.NumberOfFaces == singleDiceType.NumberOfFaces);

                if(dice != null)
                {
                    dice.Count += singleDiceType.Count;
                }
                else
                {
                    diceListToSet.Add(singleDiceType);
                }
            }

            Dice = diceListToSet.OrderByDescending(x => x.NumberOfFaces).ToList();
        }

        public override string ToString()
        {
            StringBuilder builder = new();

            foreach (Dice dice in Dice)
            {
                if(builder.Length > 0)
                {
                    builder.Append("+");
                }

                builder.Append($"{dice.Count}d{dice.NumberOfFaces}");
            }

            if (Modifier < 0 || builder.Length == 0)
            {
                builder.Append($"{Modifier}");
            }
            else if (Modifier > 0)
            {
                builder.Append($"+{Modifier}");
            }

            return builder.ToString();
        }

        public static Regex DiceRegex = new("[d]{1}[0-9]+");
        public static Regex MultiDiceRegex = new("[0-9]+[d]{1}[0-9]+");
        public static Regex ModRegex = new("[0-9]+");

        public static bool TryParse(string input, out DicePool? parsedDicePool)
        {
            try
            {
                return TryParseInternal(input, out parsedDicePool);
            }
            catch
            {
                parsedDicePool = null;

                return false;
            }
        }

        public static bool TryParseInternal(string input, out DicePool? parsedDicePool)
        {
            input = input.Replace('к', 'd');
            parsedDicePool = null;

            if (!input.IsValidDicePool())
            {
                return false;
            }
            else
            {
                parsedDicePool = new DicePool();
            }

            input = input.Replace(" ", string.Empty);
            string[] splittedString = input.Split('+');

            foreach(string entry in splittedString) 
            {
                if (MultiDiceRegex.IsMatch(entry))
                {
                    string[] splittedDice = entry.Split("d");

                    parsedDicePool.Add(
                        new Dice(
                            int.Parse(splittedDice[1]),
                            int.Parse(splittedDice[0])
                        )
                    );
                }
                else if (DiceRegex.IsMatch(entry))
                {
                    parsedDicePool.Add(
                        new Dice(int.Parse(entry.Substring(1)))
                    );
                }
                else if(ModRegex.IsMatch(entry))
                {
                    parsedDicePool.Modifier += int.Parse(entry);
                }
            }

            return true;
        }

        /// <summary>
        /// Раскладывает числовое значение на набор костей с заданным разбросом. Разброс задаётся в процентах от исходного 
        /// значения. Таким образом 10 с разбросом 0.2 дадут диапазон 8-12. Любого заданного разброса метод будет 
        /// пытаться достичь при помощи стандартных костей (1к4, 1к6, 1к8, 1к10, 1к12, 1к20), стараясь не выходить за 20 штук.
        /// </summary>
        /// <param name="value">Значение, раскладываемое на кости.</param>
        /// <param name="desiredSpreadPercent">Желаемый разброс значений в диапазоне от 0 до 1.</param>
        /// <returns></returns>
        public static DicePool FromRange(int lower, int upper, bool includeD2 = false, int maxDiceCount = 20)
        {
            if (lower < 1 || lower > upper)
            {
                throw new ArgumentException("Lower must be >= 1, and lower must be > upper.");
            }

            DicePool resultDicePool = new();

            if (lower != upper)
            {
                int spreadSize = upper - lower;
                int diceCount = 0;
                int diceFaces = 0;

                List<int> standartDiceSet = Enum.GetValues<EStandartDice>()
                    .Select(x => (int)x)
                    .Where(x => x > 2 || includeD2)
                    .OrderByDescending(x => x)
                    .ToList();

                (int Dice, int Count, int Remainder) nearestNonPerfect = (default, default, default);
                bool perfectFound = false;

                foreach (int dice in standartDiceSet)
                {
                    int spreadRemainder = spreadSize % (dice - 1);

                    if (spreadRemainder == 0 && spreadSize / (dice - 1) <= lower)
                    {
                        diceCount = spreadSize / (dice - 1);
                        diceFaces = dice;
                        resultDicePool.Add(new Dice(diceFaces, diceCount));
                        resultDicePool.Modifier = lower - diceCount;
                        perfectFound = true;

                        break;
                    }
                    else
                    {
                        bool noNearest = nearestNonPerfect.Dice == default;
                        bool reminderIsLess = spreadRemainder < nearestNonPerfect.Remainder;
                        bool diffBtwDiceAndReminderIsLess = Math.Abs(spreadRemainder - dice)
                            < Math.Abs(nearestNonPerfect.Remainder - nearestNonPerfect.Dice);

                        if (noNearest || reminderIsLess || diffBtwDiceAndReminderIsLess)
                        {
                            nearestNonPerfect = (dice, Math.Max(spreadSize / (dice - 1), 1), spreadRemainder);
                        }
                    }
                }

                if(!perfectFound)
                {
                    resultDicePool.Add(new Dice(nearestNonPerfect.Dice, nearestNonPerfect.Count));
                    resultDicePool.Modifier = lower - nearestNonPerfect.Count;
                }
            }
            else
            {
                resultDicePool.Modifier = lower;
            }

            return resultDicePool;
        }

        public static DicePool FromValue(int value, double spread)
        {
            if (value <= 0 || spread < 0 || spread >= 1)
            {
                throw new ArgumentException("Оба края диапазона должны быть больше нуля, а левый край должен быть меньше правого.");
            }

            int from = (value - value * spread).Round();
            int to = (value + value * spread).Round();

            return FromRange(from, to);
        }

        public static DicePool FromAdjusted(DicePool dicePoolToAdjust, int percent)
        {
            if (percent < -100)
            {
                throw new ArgumentOutOfRangeException("Нельзя уменьшить на процент больший 100%");
            }

            int average = dicePoolToAdjust.Average();
            double spread = (double)(average - dicePoolToAdjust.Min()) / average;

            int newAverage = (average + average * ((double)percent / 100)).Round();

            return FromValue(newAverage, spread);
        }
    }
}
