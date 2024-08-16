namespace BRIX.Library.Items
{
    public class ConsumableItem : Item
    {
        /// <summary>
        /// Стоимость в монетах. От неё зависит насколько дешевле будет способность, которая расходует этот прдемет.
        /// </summary>
        public int Price { get; set; }

        public bool IsAvailiable => Count > 0;

        /// <summary>
        /// Расчитать эквивалент стоимости в очках опыта. Показывает насколько способность будет дешевле, если станет 
        /// использовать этот расходник.
        /// </summary>
        public int ToExpEquivalent()
        {
            return Price * 4;
        }
    }
}
