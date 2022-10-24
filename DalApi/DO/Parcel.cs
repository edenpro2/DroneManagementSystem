using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace DalFacade.DO
{
    [XmlRoot]
    public class Parcel : ViewModelBase
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
        private int _senderId;
        [XmlAttribute]
        public int SenderId
        {
            get => _senderId;
            set
            {
                _senderId = value;
                OnPropertyChanged();
            }
        }
        private int _targetId;
        [XmlAttribute]
        public int TargetId
        {
            get => _targetId;
            set
            {
                _targetId = value;
                OnPropertyChanged();
            }
        }
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
        private WeightCategories _weight;
        [XmlAttribute]
        public WeightCategories Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged();
            }
        }
        private Priorities _priority;
        [XmlAttribute]
        public Priorities Priority
        {
            get => _priority;
            set
            {
                _priority = value;
                OnPropertyChanged();
            }
        }
        private DateTime _requested;
        [XmlElement]
        public DateTime Requested
        {
            get => _requested;
            set
            {
                _requested = value;
                if (_requested != default)
                    StatusIcon = "../Icons/status1.png";
                OnPropertyChanged();
            }
        }
        private DateTime _scheduled;
        [XmlElement]
        public DateTime Scheduled
        {
            get => _scheduled;
            set
            {
                _scheduled = value;
                if (_scheduled != default)
                    StatusIcon = "../Icons/status2.png";
                OnPropertyChanged();
            }
        }
        private DateTime _collected;
        [XmlElement]
        public DateTime Collected
        {
            get => _collected;
            set
            {
                _collected = value;
                if (_collected != default)
                    StatusIcon = "../Icons/status3.png";
                OnPropertyChanged();
            }
        }
        private DateTime _delivered;
        [XmlElement]
        public DateTime Delivered
        {
            get => _delivered;
            set
            {
                _delivered = value;
                if (_delivered != default)
                    StatusIcon = "../Icons/status4.png";
                OnPropertyChanged();
            }
        }

        private string _statusIcon;
        [XmlAttribute]
        public string StatusIcon
        {
            get => _statusIcon;
            set
            {
                if (_statusIcon == value) return;
                _statusIcon = value;
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
        public new event PropertyChangedEventHandler PropertyChanged;

        protected new void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public Parcel() { }

        public Parcel(int id = -1, int senderId = -1, int targetId = -1, int droneId = -1,
            WeightCategories weight = WeightCategories.Light, Priorities priority = Priorities.Regular,
            DateTime requested = default, DateTime scheduled = default, DateTime collected = default,
            DateTime delivered = default)
        {
            Id = id;
            SenderId = senderId;
            TargetId = targetId;
            DroneId = droneId;
            Weight = weight;
            Priority = priority;
            Requested = requested;
            Scheduled = scheduled;
            Collected = collected;
            Delivered = delivered;
            StatusIcon = "";
            Active = true;
        }

        public override string ToString()
        {
            return "Id: " + Id + '\n' + "Sender: " + SenderId + '\n' + "Target: " + TargetId + '\n' + "Drone: " +
                   DroneId + '\n' + "Weight: " + Weight + '\n' + "Priority: " + Priority + '\n' + "Requested: " +
                   Requested + '\n' + "Scheduled: " + Scheduled + '\n' + "Collected: " + Collected + '\n' +
                   "Delivered: " + Delivered + '\n' + "Active:" + Active;
        }
    }
}