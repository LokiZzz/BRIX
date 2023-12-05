using System.Globalization;

namespace BRIX.Mobile.Resources.Converters
{
    public class ModifierConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                return (double)value * double.Parse(parameter.ToString() ?? string.Empty);
            }
            else
            {
                return 0;
            }
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                return (double)value / double.Parse(parameter.ToString() ?? string.Empty);
            }
            else
            {
                return 0;
            }
        }
    }
}
