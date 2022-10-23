using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace PL.Converters
{
    [ValueConversion(typeof(int[]), typeof(SolidBrush))]
    public class BubbleColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            values = values.ToArray();
            var senderId = System.Convert.ToInt32(values[0]);
            var userId = System.Convert.ToInt32(values[1]);

            // light blue
            var receiverBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#d1e3ff");

            // off-white
            var senderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#ebebeb");

            if (senderId == userId)
                return senderBrush;

            else return receiverBrush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
