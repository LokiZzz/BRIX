using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Characters;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Library.Extensions;

namespace BRIX.Library.Items
{
    /// <summary>
    /// Перманентно увеличивает урон, наносимый персонажем при помощи эффекта «Урон».
    /// </summary>
    public class WeaponItem : Item
    {
        /// <summary>
        /// Мощь оружия. Выражается в бонусе, который добавляется к урону, который наносит персонаж.
        /// Может быть как константой, так и формулой костей.
        /// </summary>
        public DicePool Damage { get; set; } = new(1);

        /// <summary>
        /// Максимальная дистанция, до которой работает бонус от оружия.
        /// </summary>
        public int Distance { get; set; } = 1;

        /// <summary>
        /// Стоимость предмета в монетах. Зависит от мощи и максимальной дистанции.
        /// Рассчитывается через эквивалентый эффект «Урон».
        /// </summary>
        public int Price
        {
            get
            {
                DamageEffect effectEquivalent = new() { Impact = Damage };
                TargetSelectionAspect tsa = effectEquivalent.GetAspect<TargetSelectionAspect>();
                tsa.NTAD.DistanceInMeters = Distance;

                return effectEquivalent.GetExpCost() * 50;
            }
        }

        public int LevelRequired => CharacterCalculator.GetLevelFromExp(Price / 2);

        /// <summary>
        /// Примерно подстраивает предмет под заданный уровень. 
        /// Если не указать дистанцию, то она будет сгенерирована случайным образом от 3 до 20.
        /// </summary>
        public void TuneToPrice(int price, int distance)
        {
            Distance = distance;
            Damage = new DicePool(1);

            // Прибавляем по 1 урона к среднему, пока не достигнем желаемой стоимости.
            while (Price < price)
            {
                Damage.Modifier++;
            }

            if (Damage.Modifier >= 2)
            {
                int upperPrice = Price;
                Damage.Modifier--;
                int lowerPrice = Price;

                if (upperPrice - price <= price - lowerPrice)
                {
                    Damage.Modifier++;
                }
            }

            if (Damage.Average() > 3)
            {
                Damage = DicePool.FromValue(Damage.Average(), 0.5);
            }
        }
    }
}
