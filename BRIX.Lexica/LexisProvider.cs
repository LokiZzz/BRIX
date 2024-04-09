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
        public static async Task<string> ToLexis(this object model, CultureInfo? cultureInfo = null)
        {
            try
            {
                return await ToLexisInternal(model, cultureInfo);
            }
            catch(Exception ex)
            {
                return $"[LEXIS ERROR: {ex.Message}, {ex.StackTrace}]";
            }
        }

        public static async Task<string> ToLexisInternal(object model, CultureInfo? cultureInfo = null)
        {
            if (cultureInfo == null)
            {
                cultureInfo = Thread.CurrentThread.CurrentUICulture;
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

            if(string.IsNullOrEmpty(html))
            {
                throw new Exception("Ошибка при вызове HtmlRenderer. Вероятно, не найден шаблон или в шаблоне произошла ошибка.");
            }

            return html.Trim();
        }

        /// <summary>
        /// Находит тип шаблона (документ лексики) по полному имени в следующем формате: 
        /// BRIX.Lexica.Templates.ru_RU.NameOfModelT
        /// Папка с шаблонами должна иметь имя культуры с дефисом заменённым на подчёркивание,
        /// а шаблон иметь имя модели с буквой «T», добавленной вконце.
        /// </summary>
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
    }
}
