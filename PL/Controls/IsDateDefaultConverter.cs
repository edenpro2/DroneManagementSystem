using System;
using System.Globalization;
using System.Windows.Data;

namespace PL.Controls
{
    [ValueConversion(typeof(DateTime), typeof(bool))]
    public class IsDateDefaultConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (DateTime) value == default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Now;
        }
    }
}