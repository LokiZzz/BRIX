using System.Globalization;

namespace BRIX.Mobile.Resources.Converters
{
    public class BoolToSelectedColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool? selected = value as bool?;
            Color? result;
            object? colorObject = null;

            if (selected == true)
            {
                Application.Current?.Resources.TryGetValue("BRIXOrange", out colorObject);
                result = colorObject as Color;
            }
            else
            {
                Application.Current?.Resources.TryGetValue("BRIXDim", out colorObject);
                result = colorObject as Color;
            }

            return result ?? throw new Exception("В ресурсах не найдены BRIXOrange и BRIXDim.");
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
