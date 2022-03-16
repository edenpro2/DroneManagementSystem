using BLAPI;
using DO;
using PL.ViewModels;
using System.Windows;

namespace PL.Pages
{
    public partial class EmployeeUserInterface
    {
        private readonly BlApi _bl;
        private readonly Window _window;
        public UserViewModel ViewModel { get; }

        public EmployeeUserInterface(BlApi ibl, User user, Window window)
        {
            _bl = ibl;
            var customer = _bl.SearchForCustomer(c => c.id == user.customerId);
            ViewModel = new UserViewModel(user, customer.phone, customer.name);
            _window = window;
            InitializeComponent();
            DataContext = this;
            PagesNavigation.Navigate(new DronesPage(_bl));
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void RestoreBtn_Click(object sender, RoutedEventArgs e)
        {
            _window.WindowState =
                _window.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            _window.WindowState = WindowState.Minimized;
        }

        private void DroneBtn_Checked(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new DronesPage(_bl));
        }

        private void StationBtn_Checked(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new StationsPage(_bl));
        }

        private void CustomerBtn_Checked(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new CustomersPage(_bl));
        }

        private void ParcelBtn_Checked(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new ParcelsPage(_bl));
        }
    }
}