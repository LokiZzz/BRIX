using BRIX.Library.Aspects;
using System.Globalization;
using System.Resources;

namespace BRIX.Lexica
{
    public static class LexisProvider
    {
        public static string ToLexis(this object model, CultureInfo? cultureInfo = null)
        {
            string resourceName = model.GetType().Name;
            string resourceString = ResourceHelper.GetResourceString(resourceName);
            
            if(cultureInfo == null)
            {
                cultureInfo = Thread.CurrentThread.CurrentUICulture;
            }

            if (TryHandleSpecialConditions(model, out string formattedString, cultureInfo))
            {
                return formattedString;
            }

            return string.Format(new LexisFormatter(cultureInfo), resourceString, model);
        }

        private static bool TryHandleSpecialConditions(
            object model, out string formattedString, CultureInfo cultureInfo)
        {
            formattedString = string.Empty;

            if (model is CooldownAspect cooldownAspect && cooldownAspect.UsesCount == 0)
            {
                formattedString = ResourceHelper.GetResourceString("CooldownAspect_Special_NoneCooldown");

                return true;
            }

            return false;
        }
    }
}
