using System.Collections.Generic;
using System.Xml.Serialization;
using static DO.DegreePrinter;

namespace DO
{
    [XmlRoot(ElementName = "Station")]
    public struct Station
    {
        public static readonly short MAXCHARGESLOTS = 15;

        [XmlAttribute]
        public int id { get; set; }
        [XmlAttribute]
        public int name { get; set; }
        [XmlAttribute]
        public int openSlots { get; set; }
        [XmlElement]
        public double latitude { get; set; }
        [XmlElement]
        public double longitude { get; set; }
        [XmlArray]
        [XmlArrayItem("Charge port")]
        public List<DroneCharge> ports { get; set; }
        [XmlAttribute]
        public bool active { get; set; }

        public Station(int Id, int Name, int OpenSlots, double Latitude, double Longitude, List<DroneCharge> Ports, bool Active) : this()
        {
            id = Id;
            name = Name;
            openSlots = OpenSlots;
            latitude = Latitude;
            longitude = Longitude;
            ports = Ports;
            active = Active;
        }

        // Constructor
        public Station(int Id, int Name, int OpenSlots, double Latitude, double Longitude) : this()
        {
            id = Id;
            name = Name;
            openSlots = OpenSlots;
            latitude = Latitude;
            longitude = Longitude;
            ports = new List<DroneCharge>(MAXCHARGESLOTS);
            active = true;
        }

        // Copy Constructor
        public Station(Station source)
        {
            id = source.id;
            name = source.name;
            openSlots = source.openSlots;
            latitude = source.latitude;
            longitude = source.longitude;
            ports = new List<DroneCharge>(source.ports);
            active = source.active;
        }

        public override string ToString()
        {
            return
                "Id: " + id + '\n' +
                "Name: " + name + '\n' +
                "Available slots:" + openSlots + '\n' +
                PrintToDms(longitude, latitude) + '\n';
        }
    }
}