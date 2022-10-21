using BLAPI;
using PL.ViewModels;
using PL.Windows;
using PL.Windows.Tracking;
using System.Windows;
using System.Windows.Input;
using DalFacade.DO;

namespace PL.Pages
{
    public partial class CustomersPage
    {
        private readonly BlApi _bl;
        public CustomerViewModel CustomerViewModel { get; }
      
        public CustomersPage(BlApi ibl)
        {
            _bl = ibl;
            CustomerViewModel = new CustomerViewModel(ibl.GetCustomers(c => c.Active));
            InitializeComponent();
        }

        private void CustomerListBox_Click(object sender, MouseButtonEventArgs e)
        {
            if (CustomerListBox.SelectedItems.Count != 1)
                return;

            new CustomerWindow((Customer)CustomerListBox.SelectedItem).ShowDialog();
        }

        private void DisplayAsMap_Click(object sender, RoutedEventArgs e) => new Map(_bl, "customer").Show();
    }
}
