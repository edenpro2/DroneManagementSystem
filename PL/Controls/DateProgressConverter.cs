using System;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Data;
using System.Drawing;
using System.Linq;
using Brush = System.Windows.Media.Brush;

namespace PL.Controls
{
    [ValueConversion(typeof(DateTime[]), typeof(Brush))]
    public class DateProgressConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            values = values.ToArray();
 
            // if delivered
            if (System.Convert.ToDateTime(values[1]) != default) 
                return (Brush)new BrushConverter().ConvertFrom("#60d168"); //green
            // if current date not default
            if (System.Convert.ToDateTime(values[0]) != default) // red
                return (Brush)new BrushConverter().ConvertFrom("#f03a2e"); 
            
            return (Brush)new BrushConverter().ConvertFrom("#ffcbcbcc"); // gray
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
