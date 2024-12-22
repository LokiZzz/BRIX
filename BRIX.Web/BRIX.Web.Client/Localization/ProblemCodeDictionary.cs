namespace BRIX.Web.Client.Localization
{
    public static class ProblemCodeExtensions
    {
        public static string GetMessage(this string problemCode)
        {
            return $"Problem_{problemCode}";
        }
    }
}
