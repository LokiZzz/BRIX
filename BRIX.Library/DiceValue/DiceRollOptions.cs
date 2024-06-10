namespace BRIX.Library.DiceValue
{
    public class DiceRollOptions
    {
        /// <summary>
        /// Шанс критического значения (бросок d100)
        /// </summary>
        public int CriticalPercent { get; set; }

        /// <summary>
        /// Множитель критического значения (умножается либо результат, либо количество бросаемых костей и константа)
        /// </summary>
        public int CriticalModifier { get; set; } = 1;

        /// <summary>
        /// Если true, то умножается количество костей и константа, иначе умножается конечный результат.
        /// </summary>
        public bool RollCritical { get; set; }

        /// <summary>
        /// Значения, которые перебрасываются.
        /// </summary>
        public List<int> RerollValues { get; set; } = [];

        /// <summary>
        /// Глубина взрыва, то есть какое количество раз кости взрываются.
        /// Если взрыва нет, то глубина равна 0.
        /// </summary>
        public int ExplodeDepth { get; set; }
    }
}
