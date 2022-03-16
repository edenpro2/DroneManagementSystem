using System.Windows;

namespace PL.Pages
{
    public partial class HomePage
    {
        private CustomerUserInterfacePage CustomerUserInterfacePage { get; }

        public HomePage(CustomerUserInterfacePage customerUi)
        {
            CustomerUserInterfacePage = customerUi;
            InitializeComponent();
        }

        private void NewParcelBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerUserInterfacePage.AddPackageBtn_Click(sender, e);
        }

        private void SentBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerUserInterfacePage.TrackSentByBtn_Click(sender, e);
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerUserInterfacePage.SettingsBtn_Click(sender, e);
        }

        private void ScheduledBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerUserInterfacePage.TrackSentToBtn_Click(sender, e);
        }
    }
}