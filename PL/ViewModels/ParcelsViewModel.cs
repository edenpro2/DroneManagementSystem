using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PL.ViewModels
{
    public class ParcelsViewModel : INotifyPropertyChanged
    {
        private IEnumerable<ParcelViewModel> _parcels;

        private IEnumerable<ParcelViewModel> _filtered;

        public IEnumerable<ParcelViewModel> Parcels
        {
            get => _parcels;
            set
            {
                _parcels = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<ParcelViewModel> Filtered
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
            _filtered = _parcels = parcelList;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
