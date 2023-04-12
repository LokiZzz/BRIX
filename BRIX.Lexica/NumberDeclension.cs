using BRIX.Library.DiceValue;

namespace BRIX.Lexis
{
    public static class Numbers
    {
        /// <summary>
        /// Склонения слов, зависящие от числа.
        /// Формат: ключ: им.п., значение: им.п., род.п., род.п. и мн.ч.
        /// </summary>
        private static Dictionary<string, string[]> NumberDeclensions = new()
        {
            { "очко", new string[] { "очко", "очка", "очков" } },
            { "метр", new string[] { "метр", "метра", "метров" } },
            { "цель", new string[] { "цель", "цели", "целей" } },
            { "воксель", new string[] { "воксель", "вокселя", "вокселей" } },
        };

        /// <summary>
        /// Получить склонение от числа на русском языке
        /// </summary>
        public static string RUSDeclension(int number, string nominative, bool addNumber = true)
        {
            string[] titles = new[]
            {
                NumberDeclensions[nominative][0], // Именительный (день)
                NumberDeclensions[nominative][1], // Родительный (дня)
                NumberDeclensions[nominative][2], // Род. п., множественное (дней)
            };
            int[] cases = new[] { 2, 0, 1, 1, 1, 2 };
            int searchingСaseIndex;

            if (number % 100 > 4 && number % 100 < 20)
            {
                searchingСaseIndex = 2;
            }
            else
            {
                if (number % 10 < 5)
                {
                    searchingСaseIndex = cases[number % 10];
                }
                else
                {
                    searchingСaseIndex = cases[5];
                }
            }

            string result = titles[searchingСaseIndex];

            return addNumber ? $"{number} {result}" : result;
        }

        /// <summary>
        /// Получить склонение от числительных в формуле костей на русском языке
        /// </summary>
        public static string RUSDeclension(DicePool dicePool, string nominative)
        {
            return $"{dicePool} {RUSDeclension(dicePool.LastDigit(), nominative, false)}";
        }

        /// <summary>
        /// Получить склонение от числа на английском языке
        /// </summary>
        public static string ENGDeclension(int number, string nominative, bool addNumber = true)
        {
            string result;

            if(number != 1)
            {
                result = nominative + "s";
            }
            else
            {
                result = nominative;
            }

            return addNumber ? $"{number} {result}" : result;
        }

        /// <summary>
        /// Получить склонение от числительных в формуле костей на английском языке
        /// </summary>
        public static string ENGDeclension(DicePool dicePool, string nominative)
        {
            return $"{dicePool} {ENGDeclension(dicePool.LastDigit(), nominative, false)}";
        }

        /// <summary>
        /// Решающее числительное формулы костей для выбора склонения зависимого слова.
        /// </summary>
        public static int LastDigit(this DicePool dicePool)
        {
            return dicePool.Dice.Any()
                ? dicePool.Dice.Last().NumberOfFaces
                : dicePool.Modifier;
        }
    }
}
