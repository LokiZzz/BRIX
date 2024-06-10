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
        public int CriticalModifier { get; set; }

        /// <summary>
        /// Если true, то умножается количество костей и константа, иначе умножается конечный результат.
        /// </summary>
        public bool RollCritical { get; set; }

        /// <summary>
        /// Значения, которые перебрасываются.
        /// </summary>
        public List<int> RerollValues { get; set; } = [];

        /// <summary>
        /// Если true, то при выпадении на кости максимального значения, 
        /// она бросается снова и к результату прибавляется новый бросок.
        /// </summary>
        public bool Explode { get; set; } = false;

        /// <summary>
        /// Глубина взрыва, то есть какое количество раз кости взрываются.
        /// </summary>
        public int ExplodeDepth { get; set; }
    }
}
