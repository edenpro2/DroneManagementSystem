using System;
using System.Globalization;
using System.Windows.Data;

namespace PL.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class PlayPauseImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "/Icons/pause.png" : "/Icons/start.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strValue = (string)value;

            return strValue switch
            {
                "/Icons/pause.png" => true,
                "/Icons/start.png" => false,
                _ => false
            };
        }
    }
}