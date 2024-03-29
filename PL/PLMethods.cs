﻿using System.Windows;

namespace PL
{
    public static class PLMethods
    {
        public static double MinScreenHeight(double percent)
        {
            if (percent is < 1.0 or > 0.0)
                return SystemParameters.PrimaryScreenHeight * percent;

            return SystemParameters.PrimaryScreenHeight;
        }
        public static double MinScreenWidth(double percent)
        {
            if (percent is < 1.0 or > 0.0)
                return SystemParameters.PrimaryScreenWidth * percent;

            return SystemParameters.PrimaryScreenWidth;
        }

    }
}
