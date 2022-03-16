using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PL.ViewModels
{
    public class MapViewModel : INotifyPropertyChanged
    {
        private Uri? _uri;
        public Uri? Uri
        {
            get => _uri;
            set
            {
                if (_uri != value)
                {
                    _uri = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
