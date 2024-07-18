using BRIX.Library.Extensions;
using BRIX.Library.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public double Walk { get; set; } = _walkDefault;

        private static readonly double _swimDefault = 2.5;
        public double Swim { get; set; } = _swimDefault;

        private static readonly double _climbDefault = 2.5;
        public double Climb { get; set; } = _climbDefault;

        public double Fly { get; set; } = 0;

        public double Burrow { get; set; } = 0;

        public double Teleportation { get; set; } = 0;

        public int GetExpCost()
        {
            // Все расчёты далее происходят не в метрах, а в сантиметрах, для достижения необходимой точности.
            ThrasholdCostConverter converter = new ((1, 1), (101, 5), (301, 10), (1001, 5));

            // m/AP — это м/ОД, то есть метры расстояния за очко действия.
            // Эта величина равна 1/5 от значения скорости.
            // Для ходьбы, плавания и лазания стоимость улучшения вычисляется через прирост к стандартной скорости.

            int mapWalk = ((Walk - _walkDefault) / 5 * 100).Round();
            int walkCost = converter.Convert(mapWalk);

            int mapSwim = ((Swim - _swimDefault) / 5 * 100).Round();
            int swimCost = converter.Convert(mapSwim);

            int mapClimb = ((Climb - _climbDefault) / 5 * 100).Round();
            int climbCost = converter.Convert(mapClimb);

            int mapFly = (Fly / 5 * 100).Round();
            int flyCost = converter.Convert(mapFly);

            int mapBurrow = (Burrow / 5 * 100).Round();
            int burrowCost = converter.Convert(mapBurrow);

            int mapTeleportation = (Teleportation / 5 * 100).Round();
            int teleportationCost = converter.Convert(mapTeleportation);

            // Разные виды скорости стоят по разному.

            return walkCost
                + swimCost
                + (climbCost * 1.5).Round()
                + flyCost * 2
                + burrowCost * 2
                + teleportationCost * 4;
        }
    }
}
