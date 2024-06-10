﻿using BRIX.Library.Extensions;

namespace BRIX.Library.DiceValue
{
    public partial class DicePool
    {
        /// <summary>
        /// Конструктор для набора костей.
        /// </summary>
        /// <param name="modifier">Константа. Будет прибавлена после броска.</param>
        /// <param name="dice">Кости, добавляются в традиционном формате, например 3d6 => (3, 6).</param>
        public DicePool(int modifier, params (int, int)[] dice)
        {
            Modifier = modifier;
            SetDice(dice);
        }

        /// <summary>
        /// Конструктор для набора костей.
        /// </summary>
        /// <param name="dice">Кости, добавляются в традиционном формате, например 3d6 => (3, 6).</param>
        public DicePool(params (int, int)[] dice)
        {
            SetDice(dice);
        }

        public List<Dice> Dice { get; set; } = [];

        public int Modifier { get; set; }

        public bool IsEmpty => Dice.Count == 0 && Modifier == default;

        public int Average() => Dice.Sum(x => x.Average()).Round() + Modifier;

        public int Min() => Dice.Sum(x => x.Count) + Modifier;

        public int Max() => Dice.Sum(x => x.NumberOfFaces * x.Count) + Modifier;

        /// <summary>
        /// Добавляет в пул копию переданных костей.
        /// </summary>
        public void Add(Dice dice)
        {
            Dice.Add(new Dice(dice.NumberOfFaces, dice.Count));
            Normalize();
        }

        private void SetDice((int, int)[] dice)
        {
            Dice.Clear();

            foreach ((int, int) singleDiceType in dice)
            {
                Dice.Add(new Dice(singleDiceType.Item2, singleDiceType.Item1));
            }

            Normalize();
        }

        /// <summary>
        /// Нормализовать, то есть привести к пулу, в котором нет повторяющихся слагаемых.
        /// До нормализации:    3d6+2d8+1d8+4d6
        /// После нормализации: 3d8+7d6
        /// </summary>
        private void Normalize()
        {
            List<Dice> diceListToSet = [];

            foreach(Dice singleDiceType in Dice)
            {
                Dice? dice = diceListToSet.FirstOrDefault(x => x.NumberOfFaces == singleDiceType.NumberOfFaces);

                if(dice != null)
                {
                    dice.Count += singleDiceType.Count;
                }
                else
                {
                    diceListToSet.Add(singleDiceType);
                }
            }

            Dice = [.. diceListToSet.OrderByDescending(x => x.NumberOfFaces)];
        }
    }
}
