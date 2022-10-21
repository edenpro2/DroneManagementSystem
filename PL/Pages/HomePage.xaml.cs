using PL.ViewModels;
using PL.Windows;
using System.Windows;
using System.Windows.Media;
using PL.Controls;
using DalFacade.DO;

namespace PL.Pages
{
    public partial class HomePage
    {
        public CustomerUi CustomerUi { get; set; }

        public Parcel LatestParcel { get; set; }

        public SolidColorBrush CustomFill { get; } = new(Color.FromRgb(204, 85, 61));

        public HomePage(Parcel parcelVm, CustomerUi customerUi)
        {
            LatestParcel = parcelVm;
            CustomerUi = customerUi;
            ProgressBar = new StepProgressBar(LatestParcel);
            Test = new TestControl(LatestParcel);
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