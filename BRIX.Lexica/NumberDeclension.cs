namespace BRIX.Lexis
{
    public static class NumberDeclension
    {
        /// <summary>
        /// Склонения слов, зависящие от числа.
        /// Формат: ключ: им.п., значение: им.п., род.п., род.п. и мн.ч.
        /// </summary>
        private static Dictionary<string, string[]> NumberDeclensions = new()
        {
            { "очко", new string[] { "очко", "очка", "очков" } },
        };

        /// <summary>
        /// Получить склонение от числа
        /// </summary>
        public static string RUSDeclension(int number, string nominative, bool addNumber = false)
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

            string result = $"{number} {titles[searchingСaseIndex]}";

            return addNumber ? $"{number} {result}" : result;
        }

        public static string ENGDeclension(int number, string nominative, bool addNumber = false)
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
    }
}
