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
            private init
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