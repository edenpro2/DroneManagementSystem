using System;
using System.Globalization;
using System.Windows.Data;
using System.Linq;
using static System.Convert;

namespace PL.Controls
{
    public class DroneProgressConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            values = values.ToArray();
            var currDist = ToDouble(values[0]);
            var totalDist = ToDouble(values[1]);
            var date = ToDateTime(values[2]);
            var type = ToBoolean(values[3]);

            if (date == default)
                return 0.0;

            // here, date is not default (type == true means it is an ellipse)
            if (type)
                return 1.0;

            return currDist / totalDist;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
