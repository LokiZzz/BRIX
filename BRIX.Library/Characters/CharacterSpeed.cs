using BRIX.Library.Extensions;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Characters
{
    /// <summary>
    /// Скорость персонажа. Это расстояние, которое персонаж может преодолеть раз в раунд, дробя перемещение так, как
    /// сочтёт нужным. Дополнительно за каждое очко действия персонаж может переместиться ещё на 1/5 от этого 
    /// расстояния. При перемещении на клетках округление происходит до целого числа. Также персонаж может потратить
    /// сразу несколько очков действий, чтобы пройти за них определённое целое число клеток.
    /// </summary>
    public class CharacterSpeed
    {
        private static readonly double _walkDefault = 5;
        private static readonly double _swimDefault = 2.5;
        private static readonly double _climbDefault = 2.5;

        public double Walk { get; set; } = _walkDefault;

        public double Swim { get; set; } = _swimDefault;

        public double Climb { get; set; } = _climbDefault;

        public double Fly { get; set; } = 0;

        public double Burrow { get; set; } = 0;

        public double Teleportation { get; set; } = 0;

        public int GetExpCost()
        {
            // Все расчёты далее происходят не в метрах, а в сантиметрах, для достижения необходимой точности.
            ThrasholdCostConverter converter = new ((1, 1), (101, 2), (201, 4), (301, 2));

            int walk = ((Walk - _walkDefault) * 100).Round();
            int walkCost = converter.Convert(walk);

            int swim = ((Swim - _swimDefault) * 100).Round();
            int swimCost = converter.Convert(swim);

            int climb = ((Climb - _climbDefault) * 100).Round();
            int climbCost = converter.Convert(climb);

            int fly = (Fly * 100).Round();
            int flyCost = converter.Convert(fly);

            int burrow = (Burrow * 100).Round();
            int burrowCost = converter.Convert(burrow);

            int teleportation = (Teleportation * 100).Round();
            int teleportationCost = converter.Convert(teleportation);

            // Разные виды скорости стоят по разному.
            int cost = walkCost
                + swimCost
                + (climbCost * 1.5).Round()
                + flyCost * 2
                + burrowCost * 2
                + teleportationCost * 4;

            return cost > 0 ? cost : 0;
        }
    }
}
