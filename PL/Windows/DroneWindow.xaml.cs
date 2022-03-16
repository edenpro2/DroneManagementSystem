using BL;
using BLAPI;
using DO;
using PL.Controls;
using PL.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PL.Windows
{
    public partial class DroneWindow
    {
        private readonly BlApi _bl;
        public DroneViewModel ViewModel { get; }
        public MapViewModel MapUri { get; } = new();
        private List<DroneViewModel> _dronesViewModel;

        public DroneWindow(BlApi bl, Drone drone, List<DroneViewModel> dronesVm)
        {
            _bl = bl;
            ViewModel = new DroneViewModel(drone);
            _dronesViewModel = dronesVm.ToList();
            InitializeComponent();
            DataContext = this;
            CustomButtons = new WindowControls(this);
            UpdateContent();
            _worker = new BackgroundWorker();
        }

        private static Uri? NewMapUri(Location location)
        {
            var lat = location.latitude - location.latitude % 0.0001;
            var lon = location.longitude - location.longitude % 0.0001;

            return new Uri($"https://www.openstreetmap.org/?mlat={lat}&amp;mlon={lon}#map=11/{lat}/{lon}&amp;layers=N");
        }

        private void UpdateContent()
        {
            ViewModel.Drone = _bl.SearchForDrone(d => d.id == ViewModel.Drone.id);
            var loc = _bl.Location(ViewModel.Drone);
            MapUri.Uri = NewMapUri(loc);

        }

        private void Window_MouseLeftBtnDown(object sender, MouseButtonEventArgs e) => DragMove();

        internal List<DroneViewModel> GetValue() => _dronesViewModel;

        private void ChargeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.Drone = _bl.SendDroneToCharge(ViewModel.Drone);
                _dronesViewModel = _bl.GetDrones().Select(d => new DroneViewModel(d)).ToList();
                ErrorBox.Text = "";
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case BLDroneNotFreeException:
                        ErrorBox.Text = "Drone is currently not free";
                        break;
                    case BLNoOpenSlotsException:
                        ErrorBox.Text = "No open slots found";
                        break;
                    case BLNotEnoughBatteryException:
                        ErrorBox.Text = "Not enough battery for trip";
                        break;
                }
            }

            UpdateContent();
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
                _dronesViewModel = _bl.GetDrones().Select(d => new DroneViewModel(d)).ToList();
                ErrorBox.Text = "";
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case BLDroneNotMaintainedException:
                        ErrorBox.Text = "Drone is currently not in maintenance";
                        break;
                }
            }

            UpdateContent();
        }

        private void AssignBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.Drone = _bl.AssignDroneToParcel(ViewModel.Drone);
                _dronesViewModel = _bl.GetDrones().Select(d => new DroneViewModel(d)).ToList();
                ErrorBox.Text = "";
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case BLDroneNotFreeException:
                        ErrorBox.Text = "Drone is currently not free";
                        break;
                    default:
                        ErrorBox.Text = "No parcels awaiting collection";
                        break;
                }
            }


            UpdateContent();
        }

        private void CollectBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.Drone = _bl.CollectParcelByDrone(ViewModel.Drone);
                _dronesViewModel = _bl.GetDrones().Select(d => new DroneViewModel(d)).ToList();
                ErrorBox.Text = "";
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case BLNotFoundException:
                        ErrorBox.Text = "No collected parcel assigned to current drone";
                        break;
                    case BLNoMatchingParcels:
                        ErrorBox.Text = "No uncollected parcel found";
                        break;
                }
            }

            UpdateContent();
        }

        private void DeliverButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.Drone = _bl.DeliverByDrone(ViewModel.Drone);
                _dronesViewModel = _bl.GetDrones().Select(d => new DroneViewModel(d)).ToList();
                ErrorBox.Text = "";
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case BLNotBeingDeliveredException:
                        ErrorBox.Text = "No undelivered parcel found";
                        break;
                }
            }

            UpdateContent();
        }

        //private void RestoreBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    var maximized = new Thickness
        //    {
        //        Left = 0,
        //        Right = 0,
        //        Bottom = 0,
        //        Top = 0
        //    };

        //    var minimized = new Thickness
        //    {
        //        Left = 10,
        //        Right = 10,
        //        Bottom = 10,
        //        Top = 10
        //    };

        //    if (WindowState == WindowState.Maximized)
        //    {
        //        DesignCard.Margin = minimized;
        //        WindowState = WindowState.Normal;

        //    }
        //    else
        //    {
        //        DesignCard.Margin = maximized;
        //        WindowState = WindowState.Maximized;
        //    }
        //}

    }
}