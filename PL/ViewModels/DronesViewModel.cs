using DalFacade.DO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PL.ViewModels
{
    public class DronesViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Drone> _drones;

        private IEnumerable<Drone> _filtered;

        public IEnumerable<Drone> Drones
        {
            get => _drones;
            set
            {
                _drones = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Drone> Filtered
        {
            get => _filtered;
            set
            {
                _filtered = value;
                OnPropertyChanged();
            }
        }

        public DronesViewModel(IEnumerable<Drone> droneList)
        {
            _filtered = _drones = droneList;
        }

        public void Update(Drone drone)
        {
            _drones.ToList().Insert(drone.id, drone);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}