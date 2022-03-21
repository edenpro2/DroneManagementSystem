using System;
using System.Globalization;
using System.Windows.Data;

namespace PL.Controls
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class EmployStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (bool)value;
            return status ? "Employee" : "Customer";
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
