using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLAPI;
using DalFacade.DO;
using PL.Controls;
using PL.Pages;
using PL.ViewModels;

namespace PL.Windows
{
    public partial class CustomerUi
    {
        private const double Selected = 1.0;
        private const double Unselected = 0.5;
        private readonly BlApi _bl;
        public UserViewModel ViewModel { get; }
        private Page CurrentPage { get; set; }

        public CustomerUi(BlApi ibl, User user)
        {
            _bl = ibl;
            var customer = _bl.SearchForCustomer(c => c.id == user.customerId);
            ViewModel = new UserViewModel(user, customer.phone, customer.name);
            InitializeComponent();
            DataContext = this;
            CustomButtons = new WindowControls(this);
            HomePanel.Opacity = Selected;
            CurrentPage = new HomePage(this);
            PagesNavigation.Navigate(CurrentPage);
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            Reinitialize();
            HomePanel.Opacity = Selected;
            CurrentPage = new HomePage(this);
            PagesNavigation.Navigate(CurrentPage);
        }

        internal void AddPackageBtn_Click(object sender, RoutedEventArgs e)
        {
            Reinitialize();
            AddPanel.Opacity = Selected;
            CurrentPage = new AddParcelPage(_bl, ViewModel.User);
            PagesNavigation.Navigate(CurrentPage);
        }

        internal void TrackSentByBtn_Click(object sender, RoutedEventArgs e)
        {
            Reinitialize();
            SentPanel.Opacity = Selected;
            CurrentPage = new SentParcelsPage(_bl, ViewModel.User);
            PagesNavigation.Navigate(CurrentPage);
        }

        internal void TrackSentToBtn_Click(object sender, RoutedEventArgs e)
        {
            Reinitialize();
            TrackPanel.Opacity = Selected;
            CurrentPage = new ReceivedParcelsPage(_bl, ViewModel.User);
            PagesNavigation.Navigate(CurrentPage);
        }

        internal void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            Reinitialize();
            SettingsPanel.Opacity = Selected;
            ViewModel.User = _bl.SearchForUser(u => ViewModel.User.customerId == u.customerId);
            CurrentPage = new SettingsPage(_bl, ViewModel.User);
            PagesNavigation.Navigate(CurrentPage);
        }

        private void Reinitialize()
        {
            HomePanel.Opacity = AddPanel.Opacity =
                TrackPanel.Opacity = SentPanel.Opacity = SettingsPanel.Opacity = Unselected;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}