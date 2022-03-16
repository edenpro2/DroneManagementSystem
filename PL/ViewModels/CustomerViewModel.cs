using DalFacade.DO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static DalFacade.DO.DegreeConverter;

namespace PL.ViewModels
{
    public class CustomerViewModel : INotifyPropertyChanged
    {
        private Customer _customer;
        public Customer Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged();
            }
        }

        private string _dms;
        public string Dms
        {
            get => _dms;
            set
            {
                _dms = value;
                OnPropertyChanged();
            }
        }

        public CustomerViewModel(Customer customer)
        {
            _customer = customer;
            _dms = CoordinatesToSexagesimal(customer.longitude, customer.latitude);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
