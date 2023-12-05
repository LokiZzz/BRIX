using System.Globalization;

namespace BRIX.Mobile.Resources.Converters
{
    public class NotConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value != null ? !(bool)value : false;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value != null ? !(bool)value : false;
        }
    }
}
