using System;
using System.Windows.Input;
using DalFacade.DO;
using PL.Controls;
using PL.ViewModels;

namespace PL.Windows.Tracking
{
    public partial class StationWindow
    {
        public StationViewModel ViewModel { get; }
        public MapViewModel MapUri { get; } = new();

        public StationWindow(StationViewModel svm)
        {
            ViewModel = svm;
            InitializeComponent();
            CustomButtons = new WindowControls(this);
            DataContext = this;
            MapUri.Uri = NewMapUri(new Location(svm.Station.latitude, svm.Station.longitude));
        }

        private static Uri NewMapUri(Location location)
        {
            var lat = location.latitude - location.latitude % 0.0001;
            var lon = location.longitude - location.longitude % 0.0001;
            return new Uri($"https://www.openstreetmap.org/?mlat={lat}&amp;mlon={lon}#map=10/{lat}/{lon}&amp;layers=N");
        }

        private void Window_MouseLeftBtnDown(object sender, MouseButtonEventArgs e) => DragMove();
    }
}