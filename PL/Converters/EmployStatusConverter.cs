using System;
using System.Globalization;
using System.Windows.Data;

namespace PL.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class EmployStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Employee" : "Customer";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strValue = value as string;

            return strValue switch
            {
                "Employee" => true,
                "Customer" => false,
                _ => false
            };
        }
    }
}
