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

            if (Modifier > 0)
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
                return TryParseImpl(input, out parsedDicePool);
            }
            catch
            {
                parsedDicePool = null;

                return false;
            }
        }

        public static bool TryParseImpl(string input, out DicePool? parsedDicePool)
        {
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
    }
}
