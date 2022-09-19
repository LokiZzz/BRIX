using BRIX.Library.Extensions;

namespace BRIX.Library.Mathematics
{
    public static class NumbersExtension
    {
        public static double ToCoeficient(this int percents)
        {
            if (percents >= 0)
            {
                return 1 + (double)percents / 100;
            }
            else
            {
                if (percents > 100)
                {
                    throw new ArgumentException("Негативный коэффициент должен находится между 0 и 1.");
                }

                return (double)percents / 100;
            }
        }

        public static int ToPercent(this double coeficient)
        {
            int percent = (coeficient * 100).Round();

            return percent - 100;
        }
    }
}
