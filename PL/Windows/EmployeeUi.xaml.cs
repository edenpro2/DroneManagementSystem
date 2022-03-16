using System.Windows;
using System.Windows.Input;
using BLAPI;
using DalFacade.DO;
using PL.Pages;
using PL.ViewModels;

namespace PL.Windows
{
    public partial class EmployeeUi
    {
        private readonly BlApi _bl;
        public UserViewModel ViewModel { get; }

        public EmployeeUi(BlApi ibl, User user)
        {
            _bl = ibl;
            var customer = _bl.SearchForCustomer(c => c.id == user.customerId);
            ViewModel = new UserViewModel(user, customer.phone, customer.name);
            InitializeComponent();
            DataContext = this;
            PagesNavigation.Navigate(new DronesPage(_bl));
        }

        private void DroneBtn_Checked(object sender, RoutedEventArgs e) => PagesNavigation.Navigate(new DronesPage(_bl));

        private void StationBtn_Checked(object sender, RoutedEventArgs e) => PagesNavigation.Navigate(new StationsPage(_bl));

        private void CustomerBtn_Checked(object sender, RoutedEventArgs e) => PagesNavigation.Navigate(new CustomersPage(_bl));

        private void ParcelBtn_Checked(object sender, RoutedEventArgs e) => PagesNavigation.Navigate(new ParcelsPage(_bl));

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) => DragMove();

    }
}