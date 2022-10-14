using System;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Data;

namespace PL.Controls
{
    [ValueConversion(typeof(DateTime), typeof(Brush))]
    public class DateProgressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)value;
            return date != default ? (Brush)new BrushConverter().ConvertFrom("#f03a2e") : (Brush)new BrushConverter().ConvertFrom("#ffcbcbcc");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateVal = value as Brush;

            return DateTime.Now;
        }
    }
}
