using System.Text;
using System.Text.RegularExpressions;

namespace BRIX.Library.DiceValue
{
    public partial class DicePool
    {
        public string ToString(string format)
        {
            return format switch
            {
                "F" => ToFullString(),
                "R" => ToRollOptionsString(),
                "S" => ToShortString(),
                _ => ToString(),
            };
        }

        private string ToFullString()
        {
            string shortString = ToShortString();
            string roString = ToRollOptionsString();

            return string.IsNullOrEmpty(roString)
                ? shortString
                : $"{shortString} ({roString})";
        }

        public override string ToString()
        {
            StringBuilder builder = new();
            builder.Append(ToShortString());

            if (RollOptions.RerollValues.Count != 0)
            {
                string rerollValues = RollOptions.RerollValues
                    .Select(x => x.ToString())
                    .Aggregate((x, y) => $"{x},{y}");
                builder.Append($"; reroll:{rerollValues}");
            }

            if (RollOptions.CriticalPercent > 0)
            {
                builder.Append($"; crit:{RollOptions.CriticalPercent}x{RollOptions.CriticalModifier}");
            }

            if (RollOptions.ExplodingDepth > 0)
            {
                builder.Append($"; explode:{RollOptions.ExplodingDepth}");
            }

            return builder.ToString();
        }

        private string ToRollOptionsString()
        {
            StringBuilder builder = new();

            if (RollOptions.RerollValues.Count != 0)
            {
                string rerollValues = RollOptions.RerollValues
                    .Select(x => x.ToString())
                    .Aggregate((x, y) => $"{x},{y}");
                builder.Append($"reroll:{rerollValues}");
            }

            if (RollOptions.CriticalPercent > 0)
            {
                if (builder.Length > 0) builder.Append("; ");
                builder.Append($"crit:{RollOptions.CriticalPercent}x{RollOptions.CriticalModifier}");
            }

            if (RollOptions.ExplodingDepth > 0)
            {
                if (builder.Length > 0) builder.Append("; ");
                builder.Append($"explode:{RollOptions.ExplodingDepth}");
            }

            return builder.ToString();
        }

        private string ToShortString()
        {
            StringBuilder builder = new();

            foreach (Dice dice in Dice)
            {
                if (builder.Length > 0)
                {
                    builder.Append('+');
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

        [GeneratedRegex("[d]{1}[0-9]+")]
        public static partial Regex DiceRegex();

        [GeneratedRegex("[0-9]+[d]{1}[0-9]+")]
        public static partial Regex MultiDiceRegex();

        [GeneratedRegex("[0-9]+")]
        public static partial Regex ModRegex();

        /// <summary>
        /// Парсит строку вида 3d6+d8+4; reroll:1,2; crit:13x3; explode:2;
        /// reroll: переброс значений, указанных косле двоеточия.
        /// crit: первое число означает шанс, второе число — это модификатор,
        /// в случае прока значение умножается на второе число (модификатор), либо, если указан ключ roll,
        /// умножается только константа, а количество бросаемых костей умножается на модификатор.
        /// explode: при выпадении максимальных значений кость.
        /// Оба края диапазона возможных результатов формулы должны быть положительными.
        /// </summary>
        public static bool TryParse(string input, out DicePool? parsedDicePool)
        {
            try
            {
                if(string.IsNullOrEmpty(input) || input == "0")
                {
                    parsedDicePool = new DicePool();

                    return true;
                }

                Parse(input, out parsedDicePool);

                if(parsedDicePool?.Min() < 0 || parsedDicePool?.Max() < 0)
                {
                    parsedDicePool = null;

                    return false;
                }

                return true;
            }
            catch
            {
                parsedDicePool = null;

                return false;
            }
        }

        private static void Parse(string input, out DicePool? parsedDicePool)
        {
            input = input.Replace(" ", string.Empty);
            string[] splittedInput = input.Split(';');
            string diceFormual = splittedInput[0];
            parsedDicePool = ParseDiceFormula(diceFormual);

            string? rerollString = splittedInput.FirstOrDefault(x => x.Contains("reroll:"));
            rerollString ??= splittedInput.FirstOrDefault(x => x.Contains("r:"));

            if (!string.IsNullOrEmpty(rerollString))
            {
                List<int> rerollValues = rerollString.Split(':')[1].Split(',').Select(int.Parse).ToList();
                parsedDicePool.RollOptions.RerollValues = rerollValues;
            }

            string? critString = splittedInput.FirstOrDefault(x => x.Contains("crit:"));
            critString ??= splittedInput.FirstOrDefault(x => x.Contains("c:"));

            if (!string.IsNullOrEmpty(critString))
            {
                int percent = int.Parse(critString.Split(':')[1].Split('x')[0]);
                int modifier = int.Parse(critString.Split(':')[1].Split('x')[1]);
                parsedDicePool.RollOptions.CriticalPercent = percent;
                parsedDicePool.RollOptions.CriticalModifier = modifier;
            }

            string? explodingString = splittedInput.FirstOrDefault(x => x.Contains("explode:"));
            explodingString ??= splittedInput.FirstOrDefault(x => x.Contains("e:"));

            if (!string.IsNullOrEmpty(explodingString))
            {
                int explodingDepth = int.Parse(explodingString.Split(':')[1]);
                parsedDicePool.RollOptions.ExplodingDepth = explodingDepth;
            }
        }

        private static DicePool ParseDiceFormula(string input)
        {
            DicePool parsedDicePool = new();
            input = input.Replace('к', 'd');
            input = input.Replace("-", "+-");
            string[] splittedString = input.Split('+');

            foreach (string entry in splittedString)
            {
                if (MultiDiceRegex().IsMatch(entry))
                {
                    string[] splittedDice = entry.Split("d");

                    parsedDicePool.Add(
                        new Dice(int.Parse(splittedDice[1]), int.Parse(splittedDice[0]))
                    );
                }
                else if (DiceRegex().IsMatch(entry))
                {
                    parsedDicePool.Add(
                        new Dice(int.Parse(entry[1..]))
                    );
                }
                else if (ModRegex().IsMatch(entry))
                {
                    parsedDicePool.Modifier += int.Parse(entry);
                }
            }

            return parsedDicePool.IsEmpty
                ? throw new Exception("Неизвестный формат формулы с костями.")
                : parsedDicePool;
        }
    }
}
