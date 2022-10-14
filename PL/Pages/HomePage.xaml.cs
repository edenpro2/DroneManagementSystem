using BLAPI;
using DalFacade.DO;
using PL.ViewModels;
using PL.Windows;
using System.Windows;

namespace PL.Pages
{
    public partial class HomePage
    {
        public CustomerUi CustomerUi { get; set; }

        public ParcelViewModel LatestParcel { get; set; } 

        public HomePage(ParcelViewModel parcelvm, CustomerUi customerUi)
        {
            LatestParcel = parcelvm;
            CustomerUi = customerUi;
            InitializeComponent();
            DataContext = this;
        }

        private void NewParcelBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerUi.AddPackageBtn_Click(sender, e);
        }

        private void SentBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerUi.TrackSentByBtn_Click(sender, e);
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerUi.SettingsBtn_Click(sender, e);
        }

        private void ScheduledBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerUi.TrackSentToBtn_Click(sender, e);
        }

    }
}