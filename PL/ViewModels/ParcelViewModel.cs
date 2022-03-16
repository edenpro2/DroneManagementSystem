using BLAPI;
using DalFacade.DO;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PL.ViewModels
{
    public class ParcelViewModel : INotifyPropertyChanged
    {
        private DateTime _collected;

        private DateTime _delivered;

        private int _droneId;

        private int _id;

        private Priorities _priority;

        private DateTime _requested;

        private DateTime _scheduled;

        private int _senderId;

        private int _targetId;

        private WeightCategories _weight;

        private bool _active;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public int SenderId
        {
            get => _senderId;
            set
            {
                _senderId = value;
                OnPropertyChanged();
            }
        }

        public int TargetId
        {
            get => _targetId;
            set
            {
                _targetId = value;
                OnPropertyChanged();
            }
        }

        public int DroneId
        {
            get => _droneId;
            set
            {
                _droneId = value;
                OnPropertyChanged();
            }
        }

        public WeightCategories Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged();
            }
        }

        public Priorities Priority
        {
            get => _priority;
            set
            {
                _priority = value;
                OnPropertyChanged();
            }
        }

        public DateTime Requested
        {
            get => _requested;
            set
            {
                _requested = value;
                OnPropertyChanged();
            }
        }

        public DateTime Scheduled
        {
            get => _scheduled;
            set
            {
                _scheduled = value;
                OnPropertyChanged();
            }
        }

        public DateTime Collected
        {
            get => _collected;
            set
            {
                _collected = value;
                OnPropertyChanged();
            }
        }

        public DateTime Delivered
        {
            get => _delivered;
            set
            {
                _delivered = value;
                OnPropertyChanged();
            }
        }

        public bool Active
        {
            get => _active;
            set
            {
                _active = value;
                OnPropertyChanged();
            }
        }

        private string _dms;

        public string Dms
        {
            get => _dms;
            set
            {
                _dms = value;
                OnPropertyChanged();
            }
        }

        public string Status { get; }

        public ParcelViewModel(BlApi ibl, Parcel parcel)
        {
            _id = parcel.id;
            _senderId = parcel.senderId;
            _targetId = parcel.targetId;
            _priority = parcel.priority;
            _droneId = parcel.droneId;
            _weight = parcel.weight;
            _requested = parcel.requested;
            _scheduled = parcel.scheduled;
            _collected = parcel.collected;
            _delivered = parcel.delivered;
            _active = parcel.active;
            if (parcel.delivered != default)
            {
                Status = "../Icons/status4.png";
            }
            else if (parcel.collected != default)
            {
                Status = "../Icons/status3.png";
            }
            else if (parcel.scheduled != default)
            {
                Status = "../Icons/status2.png";
            }
            else
            {
                Status = "../Icons/status1.png";
            }

            var loc = ibl.Location(parcel);
            _dms = DegreeConverter.CoordinatesToSexagesimal(loc.longitude, loc.latitude);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}