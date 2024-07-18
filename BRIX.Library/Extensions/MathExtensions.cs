namespace BRIX.Library.Extensions
{
    public static class MathExtensions
    {
        public static int Round(this double number)
        {
            return (int)Math.Round(number, MidpointRounding.AwayFromZero);
        }

        public static double Round(this double number, int precision)
        {
            return Math.Round(number, precision, MidpointRounding.AwayFromZero);
        }
    }
}