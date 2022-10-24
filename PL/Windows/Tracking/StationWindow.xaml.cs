using DalFacade.DO;
using PL.ViewModels;
using System;
using System.Windows.Input;

namespace PL.Windows.Tracking
{
    public partial class StationWindow
    {
        public Station ViewModel { get; }
        public MapUri MapUrl { get; } = new();

        public StationWindow(Station station)
        {
            ViewModel = station;
            MapUrl.Uri = NewMapUri(new Location(ViewModel.Latitude, ViewModel.Longitude));
            InitializeComponent();
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