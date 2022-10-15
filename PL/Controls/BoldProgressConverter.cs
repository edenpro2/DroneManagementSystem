using System;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Data;
using Windows.UI.Text;

namespace PL.Controls
{
    [ValueConversion(typeof(DateTime), typeof(FontWeight))]
    public class BoldProgressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)value;
            return date != default ? new FontWeight(700) : new FontWeight(300);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Now;
        }
    }
}
