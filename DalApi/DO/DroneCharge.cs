using System.Xml.Serialization;

namespace DalFacade.DO
{
    [XmlRoot]
    public class DroneCharge : ViewModelBase
    {
        private int _droneId;
        [XmlAttribute]
        public int DroneId
        {
            get => _droneId;
            set
            {
                _droneId = value;
                OnPropertyChanged();
            }
        }
        private int _stationId;
        [XmlAttribute]
        public int StationId
        {
            get => _stationId;
            set
            {
                _stationId = value;
                OnPropertyChanged();
            }
        }

        public DroneCharge() { }

        // Constructor
        public DroneCharge(int droneId = -1, int stationId = -1)
        {
            DroneId = droneId;
            StationId = stationId;
        }

        public override string ToString()
        {
            return
                "Drone id: " + DroneId + '\n' +
                "Station id: " + StationId + '\n';
        }
    }
}