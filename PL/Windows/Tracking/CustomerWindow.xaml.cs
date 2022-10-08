using System;
using System.Windows.Input;
using DalFacade.DO;
using PL.Controls;
using PL.ViewModels;

namespace PL.Windows.Tracking
{
    public partial class CustomerWindow
    {
        public CustomerViewModel ViewModel { get; }
        public MapViewModel MapUri { get; } = new();

        public CustomerWindow(CustomerViewModel cvm)
        {
            ViewModel = cvm;
            InitializeComponent();
            CustomButtons = new WindowControls(this);
            DataContext = this;
            MapUri.Uri = NewMapUri(new Location(cvm.Customer.latitude, cvm.Customer.longitude));
        }

        private static Uri? NewMapUri(Location location)
        {
            var lat = location.latitude - location.latitude % 0.0001;
            var lon = location.longitude - location.longitude % 0.0001;
            return new Uri($"https://www.openstreetmap.org/?mlat={lat}&amp;mlon={lon}#map=10/{lat}/{lon}&amp;layers=N");
        }

        private void Window_MouseLeftBtnDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
