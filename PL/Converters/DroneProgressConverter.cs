﻿using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using static System.Convert;

namespace PL.Converters
{
    public class DroneProgressConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            values = values.ToArray();
            var currentDist = ToDouble(values[0]);
            var totalDist = ToDouble(values[1]);
            var date = ToDateTime(values[2]);
            var type = ToBoolean(values[3]);

            if (date == default)
                return 0.0;

            // here, date is not default (type == true means it is an ellipse)
            if (type)
                return 1.0;

            return currentDist / totalDist;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}