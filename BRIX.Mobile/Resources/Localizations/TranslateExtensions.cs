using BRIX.Mobile.Services;

namespace BRIX.Mobile.Resources.Localizations
{
    [ContentProperty(nameof(Name))]
    public class TranslateExtension : IMarkupExtension<BindingBase>
    {
        public string Name { get; set; } = string.Empty;
        public string StringFormat { get; set; } = string.Empty;

        public BindingBase ProvideValue(IServiceProvider serviceProvider)
        {
            return new Binding
            {
                Mode = BindingMode.OneWay,
                Path = $"[{Name}]",
                Source = Resolver.Resolve<ILocalizationResourceManager>(),
                StringFormat = StringFormat
            };
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }
    }
}
