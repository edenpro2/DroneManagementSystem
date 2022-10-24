using BL;
using PL.Pages;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using User = DalFacade.DO.User;

namespace PL.Windows
{
    public partial class CustomerUi
    {
        private BlApi _bl;
        public User ViewModel { get; set; }
        public static double MinScrHeight => PLMethods.MinScreenHeight(0.9);
        public static double MinScrWidth => PLMethods.MinScreenWidth(0.9);

        public CustomerUi(BlApi ibl, User user)
        {
            _bl = ibl;
            ViewModel = user;
            InitializeComponent();
            var latest = _bl.GetParcels(p => p.TargetId == ViewModel.Customer.Id).OrderByDescending(p => p.Requested).FirstOrDefault();
            PagesNavigation.Navigate(new HomePage(latest, this));
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            var latest = _bl.GetParcels(p => p.TargetId == ViewModel.Customer.Id).OrderByDescending(p => p.Requested).FirstOrDefault();
            PagesNavigation.Navigate(new HomePage(latest, this));
        }

        public void AddPackageBtn_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new AddParcelPage(_bl, ViewModel));
        }

        public void TrackSentByBtn_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new ParcelFlowPage(_bl, ViewModel, "sent"));
        }

        public void TrackSentToBtn_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new ParcelFlowPage(_bl, ViewModel, "received"));
        }

        public void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new SettingsPage(_bl, ViewModel));
        }

        private void ChatBtn_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new ChatsPage(_bl.GetCustomers(c => c.Active).ToList(), _bl.GetChats(), ViewModel));
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) => DragMove();
    }
}