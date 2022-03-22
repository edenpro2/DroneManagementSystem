 

using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using Color = System.Drawing.Color;
using ColorConverter = System.Drawing.ColorConverter;

namespace PL.Controls
{
    [ValueConversion(typeof(int[]), typeof(SolidBrush))]
    public class BubbleColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            values = values.ToArray();
            var senderId = System.Convert.ToInt32(values[0]);
            var userId = System.Convert.ToInt32(values[1]);

            var receiverBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#3697ff");

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
