using System;
using System.Xml.Serialization;

namespace DalFacade.DO
{
    [XmlRoot]
    public class Customer : ViewModelBase, IEquatable<Customer>
    {
        private int _id;
        [XmlAttribute] public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _name;
        [XmlAttribute] public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _phone;
        [XmlAttribute] public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }

        private Location _location;
        [XmlElement] public Location Location
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged();
            }
        }

        private bool _active;
        [XmlAttribute] public bool Active
        {
            get => _active;
            set
            {
                _active = value;
                OnPropertyChanged();
            }
        }

        // Parameter-less Ctor
        public Customer() { }

        // Constructor 
        public Customer(int id = -1, string name = "", string phone = "", Location location = default)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Location = location;
            Active = true;
        }

        // Copy Constructor 
        public Customer(Customer source)
        {
            Id = source.Id;
            Name = source.Name;
            Phone = source.Phone;
            Location = new Location(source.Location);
            Active = source.Active;
        }

        public override string ToString() =>
            $"{nameof(Id)}: {Id}\n" +
            $"{nameof(Name)}: {Name}\n" +
            $"{nameof(Phone)}: {Phone}\n" +
            $"{nameof(Location)}: {Location.ToBase60()}\n" +
            $"{nameof(Active)}: {Active}";

        public bool Equals(Customer other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return _id == other._id && 
                   _name == other._name && 
                   _phone == other._phone && 
                   Equals(_location, other._location) && 
                   _active == other._active;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;

            return Equals((Customer)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id, _name, _phone, _location, _active);
        }
    }
}