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
        public void TuneToPrice(int price, int distance = 0)
        {
            double budget = price / 10d;
            double averageForDefense = Math.Sqrt(budget / (5 * 1.25));

            Defense = averageForDefense > 0 && averageForDefense <= 3
                ? new DicePool(averageForDefense.Round())
                : DicePool.FromValue(averageForDefense.Round(), 0.5);
        }
    }
}
