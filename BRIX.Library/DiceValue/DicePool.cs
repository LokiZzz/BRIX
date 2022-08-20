using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.DiceValue
{
    public class DicePool
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

        public List<Dice> Dice { get; set; } = new List<Dice>();

        public int Modifier { get; set; }

        /// <summary>
        /// Добавляет в пул копию переданных костей.
        /// </summary>
        /// <param name="dice"></param>
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

        private void Normalize()
        {
            List<Dice> diceListToSet = new List<Dice>();

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

            Dice = diceListToSet;
        }
    }
}
