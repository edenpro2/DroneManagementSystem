using BLAPI;
using DalFacade.DO;
using PL.Controls;
using PL.Pages;
using PL.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Windows
{
    public partial class CustomerUi
    {
        private readonly BlApi _bl;
        private Page CurrentPage { get; set; }
        public static double Selected => 1.0;
        public static double Unselected => 0.5;
        public UserViewModel ViewModel { get; }
        
        public CustomerUi(BlApi ibl, User user)
        {
            _bl = ibl;
            var customer = _bl.SearchForCustomer(c => c.id == user.customerId);
            ViewModel = new UserViewModel(user, customer.phone, customer.name);
            InitializeComponent();
            DataContext = this;
            CustomButtons = new WindowControls(this);
            CurrentPage = new HomePage(this);
            PagesNavigation.Navigate(CurrentPage);
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = new HomePage(this);
            PagesNavigation.Navigate(CurrentPage);
        }

        public void AddPackageBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = new AddParcelPage(_bl, ViewModel.User);
            PagesNavigation.Navigate(CurrentPage);
        }

        public void TrackSentByBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = new ParcelFlowPage(_bl, ViewModel.User, "sent");
            PagesNavigation.Navigate(CurrentPage);
        }

        public void TrackSentToBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = new ParcelFlowPage(_bl, ViewModel.User, "received");
            PagesNavigation.Navigate(CurrentPage);
        }

        public void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.User = _bl.SearchForUser(u => ViewModel.User.customerId == u.customerId);
            CurrentPage = new SettingsPage(_bl, ViewModel.User);
            PagesNavigation.Navigate(CurrentPage);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ChatBtn_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Implement chat
        }
    }
}