using System;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Data;
using System.Drawing;
using System.Linq;

namespace PL.Controls
{
    public class DroneProgressConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            values = values.ToArray();
            var currDist = System.Convert.ToDouble(values[0]);
            var totalDist = System.Convert.ToDouble(values[1]);
            var date = System.Convert.ToDateTime(values[2]);

            if (date == default)
                return 0.0;

            return currDist / totalDist;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
