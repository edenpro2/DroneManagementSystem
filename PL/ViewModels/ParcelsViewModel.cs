using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PL.ViewModels
{
    public class ParcelsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ParcelViewModel> _parcels;

        private ObservableCollection<ParcelViewModel> _filtered;

        public ObservableCollection<ParcelViewModel> Parcels
        {
            get => _parcels;
            set
            {
                _parcels = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ParcelViewModel> Filtered
        {
            get => _filtered;
            set
            {
                _filtered = value;
                OnPropertyChanged();
            }
        }

        public ParcelsViewModel(IEnumerable<ParcelViewModel> parcelList)
        {
            _filtered = _parcels = new ObservableCollection<ParcelViewModel>(parcelList);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
