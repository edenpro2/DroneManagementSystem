using System.Windows;
using System.Windows.Input;
using BLAPI;
using DalFacade.DO;
using PL.ViewModels;
using PL.Windows;
using PL.Windows.Tracking;

namespace PL.Pages
{
    public partial class ParcelsPage
    {
        public ParcelViewModel ParcelViewModel { get; }
        private static BlApi _bl;

        public ParcelsPage(BlApi ibl)
        {
            _bl = ibl;
            ParcelViewModel = new ParcelViewModel(ibl.GetParcels());
            InitializeComponent();
        }

        private void ParcelListBox_Click(object sender, MouseButtonEventArgs e)
        {
            if (ParcelListBox.SelectedItems.Count != 1)
                return;

            new ParcelWindow(_bl, (Parcel)ParcelListBox.SelectedItem).ShowDialog();
        }

        private void DisplayAsMap_Click(object sender, RoutedEventArgs e)
        {
            new Map(_bl, "parcel").Show();
        }
    }
}