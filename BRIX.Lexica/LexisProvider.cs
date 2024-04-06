using BRIX.Library.Aspects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.HtmlRendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Reflection;

namespace BRIX.Lexica
{
    public static class LexisProvider
    {
        public static async Task<string> ToLexis2(this object model, CultureInfo? cultureInfo = null)
        {
            if (cultureInfo == null)
            {
                cultureInfo = Thread.CurrentThread.CurrentUICulture;
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }

            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            using HtmlRenderer htmlRenderer = new HtmlRenderer(serviceProvider, loggerFactory);

            string html = await htmlRenderer.Dispatcher.InvokeAsync(async () =>
            {
                Dictionary<string, object?> dictionary = new() { { "Model", model } };
                ParameterView parameters = ParameterView.FromDictionary(dictionary);

                HtmlRootComponent output = await htmlRenderer.RenderComponentAsync(GetTemplateType(model, cultureInfo), parameters);

                return output.ToHtmlString();

            });

            return html.Trim() ?? string.Empty;
        }

        private static Type GetTemplateType(object model, CultureInfo cultureInfo)
        {
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name
                ?? throw new Exception("Не удалось получить имя сборки.");
            string fullTypePath = "BRIX.Lexica.Templates." 
                + $"{cultureInfo.Name.Replace('-', '_')}." 
                + model.GetType().Name 
                + "T";
            Type? templateType = Type.GetType(Assembly.CreateQualifiedName(assemblyName, fullTypePath));

            return templateType ?? throw new NotImplementedException($"Шаблон для {model.GetType()} не найден."); 
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
