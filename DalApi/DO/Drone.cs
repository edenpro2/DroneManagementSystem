#nullable enable
using System.Xml.Serialization;

namespace DalFacade.DO
{
    [XmlRoot]
    public class Drone : ViewModelBase
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

        private string _model = "";
        [XmlElement]
        public string Model
        {
            get => _model;
            set
            {
                _model = value;
                OnPropertyChanged();
            }
        }

        private WeightCategories _maxWeight;
        [XmlElement]
        public WeightCategories MaxWeight
        {
            get => _maxWeight;
            set
            {
                _maxWeight = value;
                OnPropertyChanged();
            }
        }

        private DroneStatuses? _status;
        [XmlElement]
        public DroneStatuses? Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        private double _battery;
        [XmlElement]
        public double Battery
        {
            get => _battery;
            set
            {
                _battery = value;
                OnPropertyChanged();
            }
        }

        private Location? _location;
        [XmlElement]
        public Location? Location
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged();
            }
        }

        private string _modelImg = "";
        [XmlAttribute]
        public string ModelImg
        {
            get => _modelImg;
            set
            {
                _modelImg = value;
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
        public Drone() { }

        // Constructor
        public Drone(int id = -1, string model = "", WeightCategories maxWeight = WeightCategories.Medium, DroneStatuses? status = null, double battery = 0.0, Location? location = null)
        {
            Id = id;
            Model = model;
            MaxWeight = maxWeight;
            Status = status;
            Battery = battery;
            Location = location;
            ModelImg = "";
            Active = true;
        }

        public override string ToString()
        {
            return
                $"{nameof(Id)}: {Id}\n" +
                $"{nameof(Model)}: {Model}\n" +
                $"{nameof(MaxWeight)}: {MaxWeight}\n" +
                $"{nameof(Status)}: {Status}\n" +
                $"{nameof(Battery)}: {Battery:0:0.##}\n" +
                $"{nameof(Location)}: {Location?.ToSexagesimal()}\n" +
                $"{nameof(ModelImg)}: {ModelImg}\n" +
                $"{nameof(Active)}: {Active}";
        }
    }
}