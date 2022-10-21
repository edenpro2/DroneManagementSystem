using System.Collections.Generic;
using DalFacade.DO;
using System.Collections.ObjectModel;
using System.Linq;

namespace PL.ViewModels
{
    public class CustomerViewModel : ViewModelBase 
    {
        private ObservableCollection<Customer> _customers;
        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                OnPropertyChanged();
            }
        }

        public CustomerViewModel(IEnumerable<Customer> customers)
        {
            Customers = new ObservableCollection<Customer>(customers);
        }
    }
}
