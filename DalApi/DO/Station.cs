using System.Collections.Generic;
using System.Xml.Serialization;

namespace DalFacade.DO
{
    [XmlRoot]
    public class Station : ViewModelBase
    {
        private int _id;
        [XmlAttribute] public int Id
        {
            get => _id;
            set
            {
                if (value == _id)
                    return;

                _id = value;
                OnPropertyChanged();
            }
        }

        private int _name;
        [XmlAttribute] public int Name
        {
            get => _name;
            set
            {
                if (value == _name)
                    return;

                _name = value;
                OnPropertyChanged();
            }
        }

        private int _openSlots;
        [XmlAttribute] public int OpenSlots
        {
            get => _openSlots;
            set
            {
                if (value == _openSlots)
                    return;

                _openSlots = value;
                OnPropertyChanged();
            }
        }

        private double _latitude;
        [XmlElement] public double Latitude
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
        [XmlElement] public double Longitude
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

        private List<DroneCharge> _ports;
        [XmlArray] [XmlArrayItem("Port")]
        public List<DroneCharge> Ports
        {
            get => _ports;
            set
            {
                if (Equals(value, _ports))
                    return;

                _ports = value;
                OnPropertyChanged();
            }
        }

        private bool _active;
        [XmlAttribute] public bool Active
        {
            get => _active;
            set
            {
                if (value == _active)
                    return;

                _active = value;
                OnPropertyChanged();
            }
        }

        public const short MaxChargeSlots = 15;

        public Station() {}

        // Full Constructor
        public Station(int id, int name, int openSlots, double latitude, double longitude, List<DroneCharge> ports, bool active) 
        {
            Id = id;
            Name = name;
            OpenSlots = openSlots;
            Latitude = latitude;
            Longitude = longitude;
            Ports = ports;
            Active = active;
        }

        // Constructor
        public Station(int id = -1, int name = -1, int openSlots = -1, double latitude = 0.0, double longitude = 0.0)
        {
            Id = id;
            Name = name;
            OpenSlots = openSlots;
            Latitude = latitude;
            Longitude = longitude;
            Ports = new List<DroneCharge>(MaxChargeSlots);
            Active = true;
        }

        // Copy Constructor
        public Station(Station source)
        {
            Id = source.Id;
            Name = source.Name;
            OpenSlots = source.OpenSlots;
            Latitude = source.Latitude;
            Longitude = source.Longitude;
            Ports = new List<DroneCharge>(source.Ports);
            Active = source.Active;
        }

        public override string ToString()
        {
            return
                "Id: " + Id + '\n' +
                "Name: " + Name + '\n' +
                "Available slots:" + OpenSlots + '\n' +
                new Location(Longitude, Latitude).ToSexagesimal() + '\n';
        }
    }
}