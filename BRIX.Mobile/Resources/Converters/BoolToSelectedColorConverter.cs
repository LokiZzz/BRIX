using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Resources.Converters
{
    public class BoolToSelectedColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool selected = (bool)value;
            Color result;

            if(selected)
            {
                Application.Current.Resources.TryGetValue("BRIXOrange", out object colorObject);
                result = colorObject as Color;
            }
            else
            {
                Application.Current.Resources.TryGetValue("BRIXDim", out object colorObject);
                result = colorObject as Color;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
