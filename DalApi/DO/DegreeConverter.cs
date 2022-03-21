using System;

namespace DalFacade.DO
{
    public struct DegreeConverter
    {
        /// <summary>
        /// Special function to convert a lat and long into Degree-Minutes-Seconds format
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns>DMS string of given lat and long</returns>
        public static string CoordinatesToSexagesimal(double longitude, double latitude)
        {
            // convert each to absolute
            var absLong = Math.Abs(longitude);
            var absLat = Math.Abs(latitude);

            // extract the whole number as the degree
            var longDegree = (int)absLong;
            var latDegree = (int)absLat;

            // extract the remainder, multiply it by 60 to get the minutes
            var longMinute = (int)(absLong % 1 * 60.0);
            var latMinute = (int)(absLat % 1 * 60.0);

            // take the minutes multiplied by 60, and take the remainder of it
            var longSecond = absLong % 1 * 60.0 % 1;
            var latSecond = absLat % 1 * 60.0 % 1;

            // check for the heading (directional)
            var longDir = longitude < 0 ? 'W' : 'E';
            var latDir = latitude < 0 ? 'S' : 'N';

            return $"{longDegree}°{longMinute}'{longSecond:0.00}”{longDir} {latDegree}°{latMinute}'{latSecond:0.00}”{latDir}";

        }
    }
}