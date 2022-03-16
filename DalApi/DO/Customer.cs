using System.Xml.Serialization;
using static DalFacade.DO.DegreeConverter;

namespace DalFacade.DO
{
    [XmlRoot]
    public struct Customer
    {
        [XmlAttribute]
        public int id { get; set; }
        [XmlElement]
        public string name { get; set; }
        [XmlElement]
        public string phone { get; set; }
        [XmlAttribute]
        public double latitude { get; set; }
        [XmlAttribute]
        public double longitude { get; set; }
        [XmlAttribute]
        public bool active { get; set; }

        // Constructor
        public Customer(int Id, string name, string phone, double lat, double lon) : this()
        {
            id = Id;
            this.name = name;
            this.phone = phone;
            latitude = lat;
            longitude = lon;
            active = true;
        }

        // Copy Constructor 
        public Customer(Customer source)
        {
            id = source.id;
            name = source.name;
            phone = source.phone;
            latitude = source.latitude;
            longitude = source.longitude;
            active = source.active;
        }

        public override string ToString()
        {
            return
                "Id: " + id + '\n' +
                "Name: " + name + '\n' +
                "Phone: " + phone + '\n' +
                CoordinatesToSexagesimal(longitude, latitude) + '\n';
        }
    }
}