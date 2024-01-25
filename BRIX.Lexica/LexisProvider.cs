using System.Globalization;
using System.Resources;

namespace BRIX.Lexica
{
    public static class LexisProvider
    {
        public static string ToLexis(this object model, CultureInfo? cultureInfo = null)
        {
            string resourceName = GetResourceName(model);
            string resourceString = GetResourceString(resourceName);
            
            if(cultureInfo == null)
            {
                cultureInfo = Thread.CurrentThread.CurrentUICulture;
            }

            return string.Format(new LexisFormatter(cultureInfo), resourceString, model);
        }

        private static string GetResourceName(object model)
        {
            return model.GetType().Name;
        }

        private static string GetResourceString(string resourceName)
        {
            ResourceManager resources = new ResourceManager("BRIX.Lexica.Resources", typeof(LexisProvider).Assembly);

            return resources.GetString(resourceName) ?? string.Empty;
        }
    }
}
