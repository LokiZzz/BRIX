using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRIX.Library.DiceValue;
using BRIX.Library.Extensions;

namespace BRIX.Library.DiceValue
{
    public static class DiceValueExtensions
    {
        public static int Average(this Dice dice)
        {
            double n = dice.NumberOfFaces;
            double average = (1 + n) / 2;

            return average.Round() * dice.Count;
        }

        public static int Average(this DicePool dicePool)
        {
            return dicePool.Dice.Sum(x => x.Average()) + dicePool.Modifier; 
        }
    }
}
