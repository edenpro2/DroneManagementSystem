using BLAPI;
using PL.ViewModels;
using PL.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PL.Pages
{
    public partial class ParcelsPage
    {
        public List<ParcelViewModel> parcelsViewModel { get; }
        private readonly BlApi _bl;

        public ParcelsPage(BlApi ibl)
        {
            _bl = ibl;
            parcelsViewModel = 
                ibl.GetParcels()
                .Select(p => new ParcelViewModel(_bl, p)).ToList();

            InitializeComponent();
            DataContext = this;
        }

        private void ParcelListBox_Click(object sender, MouseButtonEventArgs e)
        {
            if (ParcelListBox.SelectedItems.Count != 1)
                return;

            var droneDetailsWindow = new ParcelWindow(_bl, (ParcelViewModel)ParcelListBox.SelectedItem);
            droneDetailsWindow.ShowDialog();
        }

        private void DisplayAsMap_Click(object sender, RoutedEventArgs e)
        {
            var map = new Map(_bl, "parcel");
            map.Show();
        }

    }
}