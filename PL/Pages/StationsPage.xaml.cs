using BL;
using DalFacade.DO;
using PL.ViewModels;
using PL.Windows;
using PL.Windows.Tracking;
using System.Windows;
using System.Windows.Input;

namespace PL.Pages
{
    public partial class StationsPage
    {
        private readonly BlApi _bl;
        public StationViewModel StationViewModel { get; }

        public StationsPage(BlApi ibl)
        {
            _bl = ibl;
            StationViewModel = new StationViewModel(ibl.GetStations(s => s.Active));
            InitializeComponent();
        }

        private void StationListBox_Click(object sender, MouseButtonEventArgs e)
        {
            new StationWindow((Station)StationListBox.SelectedItem).ShowDialog();
        }

        private void DisplayAsMap_Click(object sender, RoutedEventArgs e)
        {
            new Map(_bl, nameof(Station)).Show();
        }
    }
}