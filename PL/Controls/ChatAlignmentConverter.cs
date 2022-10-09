using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace PL.Controls
{
    [ValueConversion(typeof(int[]), typeof(HorizontalAlignment))]
    public class ChatAlignmentConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            values = values.ToArray();
            var senderId = System.Convert.ToInt32(values[0]);
            var userId = System.Convert.ToInt32(values[1]);

            if (senderId == userId)
                return HorizontalAlignment.Right;

            else return HorizontalAlignment.Left;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
