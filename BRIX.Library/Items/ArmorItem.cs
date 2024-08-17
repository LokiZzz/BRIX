using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Characters;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Library.Extensions;
using static System.Net.Mime.MediaTypeNames;

namespace BRIX.Library.Items
{
    /// <summary>
    /// Перманентно увеличивает защиту персонажа, не накладывая статусов.
    /// </summary>
    public class ArmorItem : Item
    {
        /// <summary>
        /// Мощь оружия. Выражается в бонусе, который добавляется к урону, который наносит персонаж.
        /// Может быть как константой, так и формулой костей.
        /// </summary>
        public DicePool Defense { get; set; } = new(1);

        /// <summary>
        /// Стоимость предмета в монетах. Зависит от защиты.
        /// Рассчитывается через эквивалентый эффект «Защита».
        /// </summary>
        public int Price
        {
            get
            {
                DefenseEffect effectEquivalent = new() { Impact = Defense };
                DurationAspect durationAspect = effectEquivalent.GetAspect<DurationAspect>();
                durationAspect.Duration = 10;

                return effectEquivalent.GetExpCost() * 10;
            }
        }

        public int LevelRequired => CharacterCalculator.GetLevelFromExp(Price / 2);

        /// <summary>
        /// Настраивает предмет под заданный уровень. 
        /// </summary>
        /// <summary>
        /// Примерно подстраивает предмет под заданный уровень. 
        /// Если не указать дистанцию, то она будет сгенерирована случайным образом от 3 до 20.
        /// </summary>
        public void TuneToPrice(int price)
        {
            Defense = new DicePool(1);

            // Прибавляем по 1 урона к среднему, пока не достигнем желаемой стоимости.
            while (Price < price)
            {
                Defense.Modifier++;
            }

            if (Defense.Modifier >= 2)
            {
                int upperPrice = Price;
                Defense.Modifier--;
                int lowerPrice = Price;

                if (upperPrice - price <= price - lowerPrice)
                {
                    Defense.Modifier++;
                }
            }

            if (Defense.Average() > 3)
            {
                Defense = DicePool.FromValue(Defense.Average(), 0.5);
            }
        }
    }
}
