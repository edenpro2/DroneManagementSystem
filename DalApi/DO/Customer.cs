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

        private Location _location;

        [XmlElement]
        public Location Location
        {
            get => _location;
            set
            {
                _location = value;
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
            Location = source.Location;
            Active = source.Active;
        }

        public override string ToString() =>
            $"{nameof(Id)}: {Id}\n" +
            $"{nameof(Name)}: {Name}\n" +
            $"{nameof(Phone)}: {Phone}\n" +
            $"Location: {Location.ToBase60()}\n" +
            $"{nameof(Active)}: {Active}";
    }
}