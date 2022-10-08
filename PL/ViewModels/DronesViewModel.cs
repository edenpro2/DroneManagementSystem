using DalFacade.DO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PL.ViewModels
{
    public class DronesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Drone> _drones;

        private ObservableCollection<Drone> _filtered;

        public ObservableCollection<Drone> Drones
        {
            get => _drones;
            set
            {
                _drones = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Drone> Filtered
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
            _filtered = _drones = new ObservableCollection<Drone>(droneList);
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