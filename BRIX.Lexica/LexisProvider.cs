using BRIX.Library.Aspects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.HtmlRendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace BRIX.Lexica
{
    public static class LexisProvider
    {
        public static string ToLexis2(this object model, CultureInfo? cultureInfo = null)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            using HtmlRenderer htmlRenderer = new HtmlRenderer(serviceProvider, loggerFactory);

            string html = htmlRenderer.Dispatcher.InvokeAsync(() =>
            {
                var dictionary = new Dictionary<string, object?>
                {
                    { "Model", model }
                };

                ParameterView parameters = ParameterView.FromDictionary(dictionary);
                HtmlRootComponent output = htmlRenderer.RenderComponentAsync<DamageEffectTamplate>(parameters)
                    .GetAwaiter()
                    .GetResult();

                return output.ToHtmlString();

            }).GetAwaiter().GetResult();

            return html.Trim() ?? string.Empty;
        }

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
