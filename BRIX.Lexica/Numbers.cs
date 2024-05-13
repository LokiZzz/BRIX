using BRIX.Library.DiceValue;
using Microsoft.AspNetCore.Components;

namespace BRIX.Lexica
{
    public static class Numbers
    {
        /// <summary>
        /// Короткий вызов RUSDeclension для чисел в razor-темплейтах.
        /// </summary>
        public static MarkupString RUSDcln(
            int number,
            string nominative,
            string genetive,
            string pluralGenetive,
            bool addNumber = true) => (MarkupString)RUSDeclension(number, nominative, genetive, pluralGenetive, addNumber);

        /// <summary>
        /// Получить склонение от числа на русском языке
        /// </summary>
        /// <param name="number">Число, от которого происходит склонение</param>
        /// <param name="nominative">Форма в именительном падеже (метр)</param>
        /// <param name="genetive">Форма в родительном падеже (метра)</param>
        /// <param name="pluralGenetive">Форма в родительном падеже и множественном числе (метров)</param>
        /// <param name="addNumber">Подставить ли перед склонённым словом число</param>
        /// <returns></returns>
        public static string RUSDeclension(
            int number, 
            string nominative, 
            string genetive, 
            string pluralGenetive, 
            bool addNumber = true)
        {
            string[] titles = [ nominative, genetive, pluralGenetive ];
            int[] cases = [2, 0, 1, 1, 1, 2];

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
        /// Короткий вызов RUSDeclension для костей в razor-темплейтах.
        /// </summary>
        public static MarkupString RUSDcln(
            DicePool dicePool,
            string nominative,
            string genetive,
            string pluralGenetive) => (MarkupString)RUSDeclension(dicePool, nominative, genetive, pluralGenetive);

        /// <summary>
        /// Получить склонение от числительных в формуле костей на русском языке
        /// </summary>
        public static string RUSDeclension(
            DicePool dicePool, 
            string nominative, 
            string genetive, 
            string pluralGenetive)
        {
            string result = $"{dicePool} {RUSDeclension(dicePool.LastDigit(), nominative, genetive, pluralGenetive, false)}";

            return result;
        }

        /// <summary>
        /// Короткий вызов ENGDeclension для чисел в razor-темплейтах.
        /// </summary>
        public static MarkupString ENGDcln(int number, string nominative, bool addNumber = true, string plural = "") =>
            (MarkupString)ENGDeclension(number, nominative, addNumber, plural);

        /// <summary>
        /// Получить склонение от числа на английском языке
        /// </summary>
        public static string ENGDeclension(
            int number, 
            string nominative, 
            bool addNumber = true, 
            string plural = "")
        {
            string result;

            if(number != 1)
            {
                result = string.IsNullOrEmpty(plural) ? nominative +  "s" : plural;
            }
            else
            {
                result = nominative;
            }

            return addNumber ? $"{number} {result}" : result;
        }

        /// <summary>
        /// Короткий вызов ENGDeclension для чисел в razor-темплейтах.
        /// </summary>
        public static MarkupString ENGDcln(DicePool dicePool, string nominative) =>
            (MarkupString)ENGDeclension(dicePool, nominative);

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
