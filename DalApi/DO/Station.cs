using System.Collections.Generic;
using System.Xml.Serialization;
using static DalFacade.DO.DegreeConverter;

namespace DalFacade.DO
{
    [XmlRoot(ElementName = "Station")]
    public struct Station
    {
        public const short MaxChargeSlots = 15;

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

        public Station(int id, int name, int openSlots, double latitude, double longitude, List<DroneCharge> ports, bool active) : this()
        {
            this.id = id;
            this.name = name;
            this.openSlots = openSlots;
            this.latitude = latitude;
            this.longitude = longitude;
            this.ports = ports;
            this.active = active;
        }

        // Constructor
        public Station(int id, int name, int openSlots, double latitude, double longitude) : this()
        {
            this.id = id;
            this.name = name;
            this.openSlots = openSlots;
            this.latitude = latitude;
            this.longitude = longitude;
            ports = new List<DroneCharge>(MaxChargeSlots);
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
                CoordinatesToSexagesimal(longitude, latitude) + '\n';
        }
    }
}