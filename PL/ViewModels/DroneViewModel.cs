using DO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static DO.DegreePrinter;

namespace PL.ViewModels
{
    public class DroneViewModel : INotifyPropertyChanged
    {
        private string _dms;
        public string Dms => _dms;

        private Drone _drone;

        public Drone Drone
        {
            get => _drone;
            set
            {
                _drone = value;
                if (_drone.location is not null)
                {
                    var loc = (Location)_drone.location;
                    _dms = PrintToDms(loc.longitude, loc.latitude);
                }
                OnPropertyChanged();
            }
        }

        public DroneViewModel(Drone drone)
        {
            _drone = drone;
            _dms = "";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}