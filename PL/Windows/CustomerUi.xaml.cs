using BLAPI;
using PL.Controls;
using PL.Pages;
using PL.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Windows.System;
using User = DalFacade.DO.User;

namespace PL.Windows
{
    public partial class CustomerUi
    {
        private readonly BlApi _bl;
        public UserViewModel ViewModel { get; }
        public static double MinScrHeight => PLMethods.MinScreenHeight(0.9);
        public static double MinScrWidth => PLMethods.MinScreenWidth(0.9);

        public CustomerUi(BlApi ibl, User user)
        {
            _bl = ibl;
            var customer = _bl.SearchForCustomer(c => c.id == user.customerId);
            ViewModel = new UserViewModel(user, customer.phone, customer.name, user.profilePic);
            CustomButtons = new WindowControls(this);
            InitializeComponent();
            var latest = new ParcelViewModel(_bl, _bl.GetParcels(p => p.targetId == ViewModel.User.customerId).OrderByDescending(p => p.requested).FirstOrDefault());
            PagesNavigation.Navigate(new HomePage(latest, this));
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            var latest = new ParcelViewModel(_bl, _bl.GetParcels(p => p.targetId == ViewModel.User.customerId).OrderByDescending(p => p.requested).FirstOrDefault());
            PagesNavigation.Navigate(new HomePage(latest, this));
        }

        public void AddPackageBtn_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new AddParcelPage(_bl, ViewModel.User));
        }

        public void TrackSentByBtn_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new ParcelFlowPage(_bl, ViewModel.User, "sent"));
        }

        public void TrackSentToBtn_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new ParcelFlowPage(_bl, ViewModel.User, "received"));
        }

        public void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.User = _bl.SearchForUser(u => ViewModel.User.customerId == u.customerId);
            PagesNavigation.Navigate(new SettingsPage(_bl, ViewModel.User));
        }

        private void ChatBtn_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new ChatsPage(_bl.GetCustomers(c => c.active).ToList(), _bl.GetChats(), ViewModel.User));
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) => DragMove();
    }
}