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

        private static string GetResourceString(string resourceName)
        {
            ResourceManager temp = new ResourceManager("BRIX.Lexica.Resources", typeof(LexisProvider).Assembly);

            return temp.GetString(resourceName) ?? throw new Exception($"Ресурс {resourceName} не найден.");
        }

        private static string GetResourceName(object model)
        {
            return model.GetType().Name;
        }
    }
}
