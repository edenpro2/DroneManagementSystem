using System.Xml.Serialization;

namespace DO
{
    [XmlRoot]
    public struct Location
    {
        [XmlAttribute("lat")]
        public double latitude { get; set; }
        [XmlAttribute("long")]
        public double longitude { get; set; }

        public Location(double lat = 0.0, double lon = 0.0)
        {
            latitude = lat;
            longitude = lon;
        }

        public Location(Location source)
        {
            latitude = source.latitude;
            longitude = source.longitude;
        }

        public override string ToString() => $"{longitude}, {latitude} ";
    }
}