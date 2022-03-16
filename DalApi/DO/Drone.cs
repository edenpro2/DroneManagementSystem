using System.Xml.Serialization;

#nullable enable
namespace DO
{
    [XmlRoot]
    public struct Drone
    {
        [XmlAttribute]
        public int id { get; set; }
        [XmlElement]
        public string model { get; set; }
        [XmlElement]
        public WeightCategories maxWeight { get; set; }
        [XmlElement]
        public DroneStatuses? status { get; set; }
        [XmlElement]
        public double battery { get; set; }
        [XmlElement]
        public Location? location { get; set; }
        [XmlAttribute]
        public bool active { get; set; }

        // Constructor
        public Drone(int id = -1, string model = "", WeightCategories maxWeight = WeightCategories.Medium,
            DroneStatuses? status = null, double battery = 0.0, Location? location = null) : this()
        {
            this.id = id;
            this.model = model;
            this.maxWeight = maxWeight;
            this.status = status;
            this.battery = battery;
            this.location = location;
            active = true;
        }

        public override string ToString()
        {
            return
                "Id: " + id + '\n' +
                "Model: " + model + '\n' +
                "Max Weight: " + maxWeight + '\n';
        }
    }
}