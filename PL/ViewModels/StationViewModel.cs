using DalFacade.DO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static DalFacade.DO.DegreeConverter;

namespace PL.ViewModels
{
    public class StationViewModel : INotifyPropertyChanged
    {
        private string _dms;
        private Station _station;

        public Station Station
        {
            get => _station;
            set
            {
                _station = value;
                OnPropertyChanged();
            }
        }

        public string Dms
        {
            get => _dms;
            set
            {
                _dms = value;
                OnPropertyChanged();
            }
        }

        public StationViewModel(Station station)
        {
            _station = station;
            _dms = CoordinatesToSexagesimal(station.longitude, station.latitude);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}