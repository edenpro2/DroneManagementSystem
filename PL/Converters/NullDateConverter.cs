using System;
using System.Globalization;
using System.Windows.Data;

namespace PL.Converters
{
    public class NullDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)value;

            return date != default ? date.ToString(CultureInfo.InvariantCulture): "N/A";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}