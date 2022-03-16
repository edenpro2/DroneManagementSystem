using BLAPI;
using PL.ViewModels;
using PL.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
namespace PL.Pages
{
    public partial class CustomersPage
    {
        private readonly BlApi _bl;
        public List<CustomerViewModel> customersViewModel { get; }
        //public List<CustomerViewModel> filtered { get; private set; }

        public CustomersPage(BlApi ibl)
        {
            _bl = ibl;
            customersViewModel = ibl.GetCustomers(c => c.active).Select(c => new CustomerViewModel(c)).ToList();
            InitializeComponent();
            DataContext = this;
        }

        private void CustomerListBox_Click(object sender, MouseButtonEventArgs e)
        {
            if (CustomerListBox.SelectedItems.Count != 1)
            {
                return;
            }

            var droneDetailsWindow = new CustomerWindow((CustomerViewModel)CustomerListBox.SelectedItem);
            droneDetailsWindow.ShowDialog();
        }

        private void DisplayAsMap_Click(object sender, RoutedEventArgs e)
        {
            var map = new Map(_bl, "customer");
            map.Show();

        }
    }
}
