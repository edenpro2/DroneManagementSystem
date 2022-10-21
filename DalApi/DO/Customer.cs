using System;
using System.Xml.Serialization;

namespace DalFacade.DO
{
    [XmlRoot]
    public class Customer : ViewModelBase
    {
        private int _id;

        [XmlAttribute]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _name;

        [XmlAttribute]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _phone;

        [XmlAttribute]
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }

        private double _latitude;

        [XmlAttribute]
        public double Latitude
        {
            get => _latitude;
            set
            {
                _latitude = value;
                OnPropertyChanged();
            }
        }

        private double _longitude;

        [XmlAttribute]
        public double Longitude
        {
            get => _longitude;
            set
            {
                _longitude = value;
                OnPropertyChanged();
            }
        }

        private bool _active;

        [XmlAttribute]
        public bool Active
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
        public Customer(int id = -1, string name = "", string phone = "", double lat = 0.0, double lon = 0.0)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Latitude = lat;
            Longitude = lon;
            Active = true;
        }

        // Copy Constructor 
        public Customer(Customer source)
        {
            Id = source.Id;
            Name = source.Name;
            Phone = source.Phone;
            Latitude = source.Latitude;
            Longitude = source.Longitude;
            Active = source.Active;
        }

        public override string ToString() =>
            $"{nameof(Id)}: {Id}\n" +
            $"{nameof(Name)}: {Name}\n" +
            $"{nameof(Phone)}: {Phone:(###)-###-####}\n" +
            $"Location: {new Location(Latitude, Longitude).ToSexagesimal()}\n" +
            $"{nameof(Active)}: {Active}";
    }
}