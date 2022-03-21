using PL.Windows;
using System.Windows;
using System.Windows.Input;

namespace PL.Pages
{
    public partial class HomePage
    {
        private CustomerUi customerUi { get; }

        public HomePage(CustomerUi customerUi)
        {
            this.customerUi = customerUi;
            InitializeComponent();
        }

        private void NewParcelBtn_Click(object sender, MouseButtonEventArgs e)
        {
            customerUi.AddPackageBtn_Click(sender, e);
        }

        private void SentBtn_Click(object sender, MouseButtonEventArgs e)
        {
            customerUi.TrackSentByBtn_Click(sender, e);
        }

        private void SettingsBtn_Click(object sender, MouseButtonEventArgs e)    
        {
            customerUi.SettingsBtn_Click(sender, e);
        }

        private void ScheduledBtn_Click(object sender, MouseButtonEventArgs e)
        {
            customerUi.TrackSentToBtn_Click(sender, e);
        }
    }
}