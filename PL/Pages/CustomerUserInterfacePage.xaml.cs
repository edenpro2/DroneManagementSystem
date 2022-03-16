using BLAPI;
using DO;
using PL.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PL.Pages
{
    public partial class CustomerUserInterfacePage
    {
        private const double Selected = 1.0;
        private const double Unselected = 0.5;
        private readonly BlApi _bl;
        public UserViewModel ViewModel { get; }
        private Window Window { get; }
        public Page CurrentPage { get; private set; }

        public CustomerUserInterfacePage(BlApi ibl, User user, Window window)
        {
            _bl = ibl;
            var customer = _bl.SearchForCustomer(c => c.id == user.customerId);
            ViewModel = new UserViewModel(user, customer.phone, customer.name);
            Window = window;
            DataContext = this;

            InitializeComponent();
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

        private void Reinitialize() => HomePanel.Opacity = AddPanel.Opacity = TrackPanel.Opacity = SentPanel.Opacity = SettingsPanel.Opacity = Unselected;

        private void CloseBtn_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void RestoreBtn_Click(object sender, RoutedEventArgs e) => Window.WindowState = Window.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;

        private void MinimizeBtn_Click(object sender, RoutedEventArgs e) => Window.WindowState = WindowState.Minimized;
    }
}