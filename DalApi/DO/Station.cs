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

        private Location _location;
        [XmlElement] public Location Location
        {
            get => _location;
            set
            {
                if (value.Equals(_location))
                    return;

                _location = value;
                OnPropertyChanged();
            }
        }

        private List<DroneCharge> _ports;
        [XmlArray]
        [XmlArrayItem("Port")] 
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

        private string _address;
        [XmlAttribute] public string Address
        {
            get => _address;
            set
            {
                if (value == _address)
                    return;

                _address = value;
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

        public Station() { }

        // Full Constructor
        public Station(int id, int name, int openSlots, Location location, List<DroneCharge> ports, string address, bool active)
        {
            Id = id;
            Name = name;
            OpenSlots = openSlots;
            Location = location;
            Ports = ports;
            Address = address;
            Active = active;
        }

        // Constructor
        public Station(int id = -1, int name = -1, int openSlots = -1, Location location = default)
        {
            Id = id;
            Name = name;
            OpenSlots = openSlots;
            Location = location;
            Ports = new List<DroneCharge>(MaxChargeSlots);
            Address = "";
            Active = true;
        }

        // Copy Constructor
        public Station(Station source)
        {
            Id = source.Id;
            Name = source.Name;
            OpenSlots = source.OpenSlots;
            Location = source.Location;
            Ports = new List<DroneCharge>(source.Ports);
            Address = source.Address;
            Active = source.Active;
        }

        public override string ToString()
        {
            return
                "Id: " + Id + '\n' +
                "Name: " + Name + '\n' +
                "Available slots:" + OpenSlots + '\n' +
                Location.ToBase60() + '\n';
        }
    }
}