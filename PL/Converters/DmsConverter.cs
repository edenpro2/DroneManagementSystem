using DalFacade.DO;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PL.Converters
{
    public class DmsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Location)value).ToBase60();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}