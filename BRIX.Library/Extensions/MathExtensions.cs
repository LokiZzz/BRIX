using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Extensions
{
    public static class MathExtensions
    {
        public static int Round(this double number)
        {
            return (int)Math.Round(number, MidpointRounding.AwayFromZero);
        }
    }
}
