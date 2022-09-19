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
