using PL.Controls;
using System.Windows;
using System.Windows.Input;
using PL.ViewModels;
using DalFacade.DO;
using BL.BO;
using BLAPI;
using System;
using BL;
using System.Linq;

namespace PL.Windows.Tracking
{
    public partial class DroneTrackingWindow 
    {
        public static double MinScreenHeight => PLMethods.MinScreenHeight(0.9);
        public static double MinScreenWidth => PLMethods.MinScreenWidth(0.9);

        private readonly BlApi _bl;
        public MapViewModel MapUri { get; } = new();

        private readonly DronesViewModel _dronesViewModel;
        public DroneViewModel ViewModel { get; set; }
        public ParcelViewModel Parcel { get; set; }
 
        public double totalDist { get; set; } 
        public double currentDist { get; set; }

        public bool IsEllipse => true;
        public bool IsRect => false;

        public DroneTrackingWindow(BlApi bl, Drone drone, DronesViewModel drones)
        {
            _bl = bl;
            ViewModel = new DroneViewModel(drone);
            _dronesViewModel = drones;
            Parcel = new ParcelViewModel(_bl, _bl.GetParcels().FirstOrDefault(p => p.active && p.droneId == ViewModel.Drone.id));
            InitializeComponent();
            UpdateContent();
            CustomButtons = new WindowControls(this);
        }

        private static Uri? NewMapUri(Location location)
        {
            var lat = location.latitude - location.latitude % 0.0001;
            var lon = location.longitude - location.longitude % 0.0001;

            return new Uri($"https://www.openstreetmap.org/?mlat={lat}&amp;mlon={lon}#map=15/{lat}/{lon}");
        }

        private void UpdateContent()
        {
            ViewModel.Drone = _bl.SearchForDrone(d => d.id == ViewModel.Drone.id);
            Parcel = new ParcelViewModel(_bl, _bl.GetParcels().FirstOrDefault(p => p.active && p.droneId == ViewModel.Drone.id));
            var loc = _bl.Location(ViewModel.Drone);
            MapUri.Uri = NewMapUri(loc);
        }

        private void Window_MouseLeftBtnDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        internal DronesViewModel GetValue() => _dronesViewModel;

        private void ChargeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.Drone = _bl.SendDroneToCharge(ViewModel.Drone);
                _dronesViewModel.Update(ViewModel.Drone);
                ErrorBox.Text = "";
                UpdateContent();
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case BlDroneNotFreeException:
                        ErrorBox.Text = "Drone is currently not free";
                        break;
                    case BlNoOpenSlotsException:
                        ErrorBox.Text = "No open slots found";
                        break;
                    case BlNotEnoughBatteryException:
                        ErrorBox.Text = "Not enough battery for trip";
                        break;
                }
            }
        }

        private void ReleaseBtn_Click(object sender, RoutedEventArgs e)
        {
            //Todo: implement number of hours

            const int hours = 1;
            //try
            //{
            //    int.TryParse(HourBox.Text, out Hours);
            //}
            //catch
            //{
            //    return;
            //}

            try
            {
                ViewModel.Drone = _bl.DroneReleaseAndCharge(ViewModel.Drone, hours);
                _dronesViewModel.Update(ViewModel.Drone);
                ErrorBox.Text = "";
                UpdateContent();
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case BlDroneNotMaintainedException:
                        ErrorBox.Text = "Drone is currently not in maintenance";
                        break;
                }
            }
        }

        private void AssignBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.Drone = _bl.AssignDroneToParcel(ViewModel.Drone);
                _dronesViewModel.Update(ViewModel.Drone);
                ErrorBox.Text = "";
                UpdateContent();
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case BlDroneNotFreeException:
                        ErrorBox.Text = "Drone is currently not free";
                        break;
                    default:
                        ErrorBox.Text = "No parcels awaiting collection";
                        break;
                }
            }
        }

        private void CollectBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.Drone = _bl.CollectParcelByDrone(ViewModel.Drone);
                _dronesViewModel.Update(ViewModel.Drone);
                ErrorBox.Text = "";
                UpdateContent();
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case BlNotFoundException:
                        ErrorBox.Text = "No collected parcel assigned to current drone";
                        break;
                    case BlNoMatchingParcels:
                        ErrorBox.Text = "No uncollected parcel found";
                        break;
                }
            }
        }

        private void DeliverButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.Drone = _bl.DeliverByDrone(ViewModel.Drone);
                _dronesViewModel.Update(ViewModel.Drone);
                ErrorBox.Text = "";
                UpdateContent();
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case BlNotBeingDeliveredException:
                        ErrorBox.Text = "No undelivered parcel found";
                        break;
                }
            }
        }
    }
}
