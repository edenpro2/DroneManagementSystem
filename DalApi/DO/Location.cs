using System;
using System.Xml.Serialization;

namespace DalFacade.DO
{
    [XmlRoot]
    public class Location : ViewModelBase
    {
        private double _latitude;

        [XmlAttribute("Lat")] public double Latitude
        {
            get => _latitude;
            set
            {
                if (value.Equals(_latitude))
                    return;

                _latitude = value;
                OnPropertyChanged();
            }
        }

        private double _longitude;

        [XmlAttribute("Long")] public double Longitude
        {
            get => _longitude;
            set
            {
                if (value.Equals(_longitude))
                    return;

                _longitude = value;
                OnPropertyChanged();
            }
        }

        public Location() {}

        public Location(double lat = 0.0, double lon = 0.0)
        {
            Latitude = lat;
            Longitude = lon;
        }

        public Location(Location source)
        {
            Latitude = source.Latitude;
            Longitude = source.Longitude;
        }

        public override string ToString()
        {
            return $"Lon: {Longitude}, Lat: {Latitude}";
        }

        public string ToSexagesimal()
        {
            // convert each to absolute
            var absLong = Math.Abs(Longitude);
            var absLat = Math.Abs(Latitude);

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
            var longDir = Longitude < 0 ? 'W' : 'E';
            var latDir = Latitude < 0 ? 'S' : 'N';

            return $"{longDegree}°{longMinute}'{longSecond:0.00}”{longDir} {latDegree}°{latMinute}'{latSecond:0.00}”{latDir}";
        }
    }
}