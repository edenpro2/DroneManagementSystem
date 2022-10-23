using System;
using System.Globalization;
using System.Windows.Data;

namespace PL.Converters
{
    public class LocationTruncator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value - (double)value % 0.0001;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}