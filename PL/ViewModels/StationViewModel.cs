using DalFacade.DO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PL.ViewModels
{
    public class StationViewModel : ViewModelBase
    {
        private ObservableCollection<Station> _stations;
        public ObservableCollection<Station> Stations
        {
            get => _stations;
            set
            {
                _stations = value;
                OnPropertyChanged();
            }
        }

        public StationViewModel(IEnumerable<Station> stations)
        {
            Stations = new ObservableCollection<Station>(stations);
        }
    }
}