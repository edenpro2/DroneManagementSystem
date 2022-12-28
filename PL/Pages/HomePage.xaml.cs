using DalFacade.DO;
using PL.Windows;
using System.Windows;
using System.Windows.Media;

namespace PL.Pages
{
    public partial class HomePage
    {
        public CustomerUi CustomerUi { get; set; }

        public Parcel? LatestParcel { get; set; }

        public SolidColorBrush CustomFill { get; } = new(Color.FromRgb(204, 85, 61));

        public HomePage(Parcel? parcelVm, CustomerUi customerUi)
        {
            LatestParcel = parcelVm;
            CustomerUi = customerUi;
            InitializeComponent();
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