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

        public string StatusIcon { get; }

        public ParcelViewModel()
        {
            _dms = "";
            StatusIcon = "";
        }

        public ParcelViewModel(BlApi ibl, Parcel parcel)
        {
            _id = parcel.Id;
            _senderId = parcel.SenderId;
            _targetId = parcel.TargetId;
            _priority = parcel.Priority;
            _droneId = parcel.DroneId;
            _weight = parcel.Weight;
            _requested = parcel.Requested;
            _scheduled = parcel.Scheduled;
            _collected = parcel.Collected;
            _delivered = parcel.Delivered;
            _active = parcel.Active;
            if (parcel.Delivered != default)
            {
                StatusIcon = "../Icons/status4.png";
            }
            else if (parcel.Collected != default)
            {
                StatusIcon = "../Icons/status3.png";
            }
            else if (parcel.Scheduled != default)
            {
                StatusIcon = "../Icons/status2.png";
            }
            else
            {
                StatusIcon = "../Icons/status1.png";
            }

            var loc = ibl.Location(parcel);
            _dms = loc.ToSexagesimal();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Converter
        public bool IsRequested => Requested != default;
        public bool IsScheduled => Scheduled != default;
        public bool IsCollected => Collected != default;
        public bool IsDelivered => Delivered != default;
        #endregion

    }
}