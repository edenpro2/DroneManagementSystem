using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

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
            var receiverBrush = new SolidColorBrush(Color.FromRgb(209, 227, 255));

            // off-white
            var senderBrush = new SolidColorBrush(Color.FromRgb(235, 235, 235)); 

            if (senderId == userId)
                return senderBrush;

            return receiverBrush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
