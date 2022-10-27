using BL;
using BL.BO.OSM;
using DalFacade.DO;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace PL.Windows.Tracking
{
    public partial class StationWindow
    {
        public Station ViewModel { get; }
        public Uri MapUrl { get; init; }
        private static BackgroundWorker Worker { get; } = new();

        public StationWindow(BlApi bl, Station station)
        {
            ViewModel = station;
            Worker.WorkerSupportsCancellation = true;
            Worker.DoWork += (_, _) => ViewModel.Address = Extensions.LocationAddress(ViewModel.Location);
            Worker.RunWorkerAsync();
            MapUrl = NewMapUri(ViewModel.Location);
            InitializeComponent();
            // Update address (from Nominatim)
            bl.UpdateStation(ViewModel);
        }

        private static Uri NewMapUri(Location location)
        {
            var lat = location.Latitude - location.Latitude % 0.0001;
            var lon = location.Longitude - location.Longitude % 0.0001;
            return new Uri($"https://www.openstreetmap.org/?mlat={lat}&amp;mlon={lon}#map=10/{lat}/{lon}&amp;layers=N");
        }

        private void Window_MouseLeftBtnDown(object sender, MouseButtonEventArgs e) => DragMove();
    }
}