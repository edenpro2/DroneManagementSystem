using BLAPI;
using DalFacade.DO;
using PL.ViewModels;
using System.Linq;
using System.Windows;

namespace PL.Pages
{
    public partial class ParcelFlowPage
    {
        public ParcelsViewModel parcels { get; }
        public string Header { get; }
        public string Intro { get; }

        public ParcelFlowPage(BlApi ibl, User user, string type)
        {
            switch (type)
            {
                case "sent":
                    Header = SentHeader;
                    Intro = SentIntro;
                    parcels = new ParcelsViewModel(ibl.GetParcels(p => p.senderId == user.customerId).Select(p => new ParcelViewModel(ibl, p)));
                    break;
                case "received":
                    Header = ReceivedHeader;
                    Intro = ReceivedIntro;
                    parcels = new ParcelsViewModel(ibl.GetParcels(p => p.targetId == user.customerId).Select(p => new ParcelViewModel(ibl, p)));
                    break;
                default:
                    Header = SentHeader;
                    Intro = SentIntro;
                    parcels = new ParcelsViewModel(ibl.GetParcels(p => p.senderId == user.customerId).Select(p => new ParcelViewModel(ibl, p)));
                    break;
            }

            InitializeComponent();
            // Must set data context, else parcels don't appear
            DataContext = this;
        }

        private void AllOrders_Click(object sender, RoutedEventArgs e) => parcels.Filtered = parcels.Parcels;

        private void Completed_Click(object sender, RoutedEventArgs e) => parcels.Filtered = parcels.Parcels.Where(p => p.Delivered != default).ToList();

        private void Ongoing_Click(object sender, RoutedEventArgs e) => parcels.Filtered = parcels.Parcels.Where(p => p.Delivered == default && p.Requested != default).ToList();

        private void Pending_Click(object sender, RoutedEventArgs e) => parcels.Filtered = parcels.Parcels.Where(p => p.Scheduled == default).ToList();

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