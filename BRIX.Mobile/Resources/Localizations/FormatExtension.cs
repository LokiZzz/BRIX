using BRIX.Mobile.Services;
using System.Collections.ObjectModel;
using System.Globalization;

namespace BRIX.Mobile.Resources.Localizations
{
    [ContentProperty(nameof(ValuePath))]
    public class FormatExtension : IMarkupExtension<BindingBase>
    {
        public string ValuePath { get; set; } = string.Empty;
        public string FormatPath { get; set; } = string.Empty;

        public BindingBase ProvideValue(IServiceProvider serviceProvider)
        {
            return new MultiBinding
            {
                Mode = BindingMode.OneWay,
                Bindings = new Collection<BindingBase>
                {
                    new Binding
                    {
                        Mode = BindingMode.OneWay,
                        Path = $"[{FormatPath}]",
                        Source = Resolver.Resolve<ILocalizationResourceManager>()
                    },
                    new Binding
                    {
                        Mode = BindingMode.OneWay,
                        Path = ValuePath,
                    }
                },
                Converter = new FormatConverter()
            };
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        private class FormatConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                return string.Format((string)values[0], values[1]);
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
