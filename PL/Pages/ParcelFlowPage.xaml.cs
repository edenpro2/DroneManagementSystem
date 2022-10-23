using BLAPI;
using DalFacade.DO;
using PL.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace PL.Pages
{
    public partial class ParcelFlowPage
    {
        public ParcelViewModel Parcel { get; }
        public string Header { get; }
        public string Intro { get; }

        public ParcelFlowPage(BlApi ibl, User user, string type)
        {
            switch (type)
            {
                case "sent":
                    Header = SentHeader;
                    Intro = SentIntro;
                    Parcel = new ParcelViewModel(ibl.GetParcels(p => p.SenderId == user.Customer.Id));
                    break;
                case "received":
                    Header = ReceivedHeader;
                    Intro = ReceivedIntro;
                    Parcel = new ParcelViewModel(ibl.GetParcels(p => p.TargetId == user.Customer.Id));
                    break;
                default:
                    Header = SentHeader;
                    Intro = SentIntro;
                    Parcel = new ParcelViewModel(ibl.GetParcels(p => p.SenderId == user.Customer.Id));
                    break;
            }

            InitializeComponent();
        }

        private void AllOrders_Click(object sender, RoutedEventArgs e)
        {
            Parcel.Filtered = Parcel.Parcels;
        }

        private void Completed_Click(object sender, RoutedEventArgs e)
        {
            Parcel.Filtered = new ObservableCollection<Parcel>(Parcel.Parcels.Where(p => p.Delivered != default));
        }

        private void Ongoing_Click(object sender, RoutedEventArgs e)
        {
            Parcel.Filtered = new ObservableCollection<Parcel>(Parcel.Parcels.Where(p => p.Delivered == default && p.Requested != default));
        }

        private void Pending_Click(object sender, RoutedEventArgs e)
        {
            Parcel.Filtered = new ObservableCollection<Parcel>(Parcel.Parcels.Where(p => p.Scheduled == default));
        }

        private const string SentHeader = "Sent Parcels";

        private const string ReceivedHeader = "Received Parcels";

        // Two types of intros, for two types of parcel flow pages

        private const string SentIntro =
            "In the order details section, you can review and manage all orders you've sent with " +
            "their information. You can view their details, such as their IDs, ordered product, price and status. " +
            "Access to this area is limited. Only company management can apply changes. The changes you make will be approved after they are checked.";

        private const string ReceivedIntro =
            "In the order details section, you can review and manage all orders that are sent to you with their information. " +
            "You can view their details, such as their IDs, ordered product, price and status. " +
            "Access to this area is limited. Only company management can apply changes. The changes you make will be approved after they are checked.";
    }
}