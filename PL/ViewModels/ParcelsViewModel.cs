using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DalFacade.DO;

namespace PL.ViewModels
{
    public class ParcelsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Parcel> _parcels;

        private ObservableCollection<Parcel> _filtered;

        public ObservableCollection<Parcel> Parcels
        {
            get => _parcels;
            set
            {
                _parcels = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Parcel> Filtered
        {
            get => _filtered;
            set
            {
                _filtered = value;
                OnPropertyChanged();
            }
        }

        public ParcelsViewModel(IEnumerable<Parcel> parcelList)
        {
            _filtered = _parcels = new ObservableCollection<Parcel>(parcelList);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
