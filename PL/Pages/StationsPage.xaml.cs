using BLAPI;
using PL.ViewModels;
using PL.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PL.Pages
{
    public partial class StationsPage
    {
        private readonly BlApi _bl;
        public List<StationViewModel> stationsViewModel { get; }

        public StationsPage(BlApi ibl)
        {
            _bl = ibl;
            stationsViewModel = ibl.GetStations(s => s.active)
                .Select(s => new StationViewModel(s)).ToList();
            InitializeComponent();
            DataContext = this;
        }

        private void StationListBox_Click(object sender, MouseButtonEventArgs e)
        {
            if (StationListBox.SelectedItems.Count != 1)
            {
                return;
            }

            var stationDetailsWindow = new StationWindow((StationViewModel)StationListBox.SelectedItem);
            stationDetailsWindow.ShowDialog();
        }

        private void DisplayAsMap_Click(object sender, RoutedEventArgs e)
        {
            var map = new Map(_bl, "station");
            map.Show();

        }
    }
}