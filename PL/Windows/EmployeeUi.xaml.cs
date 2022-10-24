using BL;
using DalFacade.DO;
using PL.Pages;
using System.Windows;
using System.Windows.Input;

namespace PL.Windows
{
    public partial class EmployeeUi
    {
        private readonly BlApi _bl;
        public User ViewModel { get; }
        public static double MinScreenHeight => PLMethods.MinScreenHeight(0.9);
        public static double MinScreenWidth => PLMethods.MinScreenWidth(0.9);

        public EmployeeUi(BlApi ibl, User user)
        {
            _bl = ibl;
            ViewModel = user;
            InitializeComponent();
            PagesNavigation.Navigate(new DronesPage(_bl));
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

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}