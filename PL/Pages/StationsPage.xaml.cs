using BLAPI;
using PL.ViewModels;
using PL.Windows;
using PL.Windows.Tracking;
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
            stationsViewModel = ibl.GetStations(s => s.active).Select(s => new StationViewModel(s)).ToList();
            InitializeComponent();
        }

        private void StationListBox_Click(object sender, MouseButtonEventArgs e)
        {
            new StationWindow((StationViewModel)StationListBox.SelectedItem).ShowDialog();
        }

        private void DisplayAsMap_Click(object sender, RoutedEventArgs e)
        {
            new Map(_bl, "station").Show();
        }
    }
}