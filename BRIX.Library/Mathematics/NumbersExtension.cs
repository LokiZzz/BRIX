using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Mathematics
{
    public static class NumbersExtension
    {
        public static double ToPositiveCoeficient(this int percents)
        {
            return 1 + percents / 100;
        }

        public static double ToNegativeCoeficient(this int percents)
        {
            if(percents > 100)
            {
                throw new ArgumentException("Негативный коэффициент должен находится между 0 и 1.");
            }

            return percents / 100;
        }
    }
}
