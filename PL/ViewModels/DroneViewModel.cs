using DalFacade.DO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PL.ViewModels
{
    public class DroneViewModel : ViewModelBase
    {
        private ObservableCollection<Drone> _drones;
        public ObservableCollection<Drone> Drones
        {
            get => _drones;
            set
            {
                _drones = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Drone> _filtered;
        public ObservableCollection<Drone> Filtered
        {
            get => _filtered;
            set
            {
                _filtered = value;
                OnPropertyChanged();
            }
        }

        public DroneViewModel(IEnumerable<Drone> droneList)
        {
            _filtered = _drones = new ObservableCollection<Drone>(droneList);
        }

        public void Update(Drone drone) => _drones.Insert(drone.Id, drone);
    }
}