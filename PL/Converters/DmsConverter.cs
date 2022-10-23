using System;
using System.Globalization;
using System.Windows.Data;
using DalFacade.DO;

namespace PL.Converters
{
    public class DmsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new Location((double)values[0], (double)values[1]).ToSexagesimal();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}