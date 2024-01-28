using BRIX.Library.Aspects;
using System.Globalization;
using System.Resources;

namespace BRIX.Lexica
{
    public static class LexisProvider
    {
        public static string ToLexis(this object model, CultureInfo? cultureInfo = null)
        {
            if (cultureInfo == null)
            {
                cultureInfo = Thread.CurrentThread.CurrentUICulture;
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }

            string resourceName = model.GetType().Name;
            string resourceString = ResourceHelper.GetResourceString(resourceName);
            string resultString = string.Format(new LexisFormatter(cultureInfo), resourceString, model);
            resultString = HandleSpecialConditions(model, resultString, cultureInfo);

            return resultString;
        }

        private static string HandleSpecialConditions(object model, string formattedString, CultureInfo cultureInfo)
        {
            if (model is CooldownAspect ca && ca.UsesCount == 0)
            {
                formattedString = ResourceHelper.GetResourceString("CooldownAspect_Special_None");
            }

            if (model is ActivationConditionsAspect aca && aca.Conditions.Count() == 0)
            {
                formattedString = ResourceHelper.GetResourceString("ActivationConditionsAspect_Special_None");
            }

            if (model is DurationAspect da && da.CanDisableStatus)
            {
                formattedString += " " + ResourceHelper.GetResourceString("DurationAspect_Special_CanDisable");
            }

            return formattedString;
        }
    }
}
