using DalFacade.DO;
using PL.Controls;
using PL.ViewModels;
using System;
using System.Windows.Input;

namespace PL.Windows.Tracking
{
    public partial class CustomerWindow
    {
        public Customer ViewModel { get; }
        public MapUri MapUrl { get; } = new();
        public static double MinScreenHeight => PLMethods.MinScreenHeight(0.30);
        public static double MinScreenWidth => PLMethods.MinScreenWidth(0.40);

        public CustomerWindow(Customer customer)
        {
            ViewModel = customer;
            MapUrl.Uri = NewMapUri(new Location(ViewModel.Latitude, ViewModel.Longitude));
            InitializeComponent();
        }

        private static Uri NewMapUri(Location location)
        {
            var lat = location.Latitude - location.Latitude % 0.0001;
            var lon = location.Longitude - location.Longitude % 0.0001;
            return new Uri($"https://www.openstreetmap.org/?mlat={lat}&amp;mlon={lon}#map=10/{lat}/{lon}&amp;layers=N");
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) => DragMove();
    }
}
