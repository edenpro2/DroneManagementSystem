using System.Xml.Serialization;

namespace DalFacade.DO
{
    [XmlRoot]
    public struct DroneCharge
    {
        [XmlAttribute]
        public int droneId { get; set; }
        [XmlAttribute]
        public int stationId { get; set; }

        // Constructor
        public DroneCharge(int droneId, int stationId)
        {
            this.droneId = droneId;
            this.stationId = stationId;
        }

        public override string ToString()
        {
            return
                "Drone id: " + droneId + '\n' +
                "Station id: " + stationId + '\n';
        }
    }
}