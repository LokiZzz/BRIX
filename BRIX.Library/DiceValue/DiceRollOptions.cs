namespace BRIX.Library.DiceValue
{
    public class DiceRollOptions
    {
        /// <summary>
        /// Шанс критического значения (бросок d100), по-умолчанию 0.
        /// </summary>
        public int CriticalPercent { get; set; }

        /// <summary>
        /// Множитель критического значения (умножается либо результат, либо количество бросаемых костей и константа).
        /// По-умолчанию равен 1.
        /// </summary>
        public int CriticalModifier { get; set; } = 1;

        /// <summary>
        /// Значения, которые перебрасываются.
        /// </summary>
        public List<int> RerollValues { get; set; } = [];

        /// <summary>
        /// Глубина взрыва, то есть какое количество раз кости взрываются.
        /// Если взрыва нет, то глубина равна 0.
        /// </summary>
        public int ExplodingDepth { get; set; }

        public void CopyPropertiesFrom(DiceRollOptions diceRollOptions)
        {
            CriticalPercent = diceRollOptions.CriticalPercent;
            CriticalModifier = diceRollOptions.CriticalModifier;
            RerollValues = diceRollOptions.RerollValues;
            ExplodingDepth = diceRollOptions.ExplodingDepth;
        }
    }
}
