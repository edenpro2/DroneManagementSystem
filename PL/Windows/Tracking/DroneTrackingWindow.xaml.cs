using BL;
using BLAPI;
using DalFacade.DO;
using PL.Controls;
using PL.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts.Wpf;

#pragma warning disable CS8602 // All null values initialized in constructor

namespace PL.Windows.Tracking
{
    public partial class DroneTrackingWindow : INotifyPropertyChanged
    {
        #region Properties
        public static double MinScreenHeight => PLMethods.MinScreenHeight(0.9);
        public static double MinScreenWidth => PLMethods.MinScreenWidth(0.9);
        public SolidColorBrush CustomFill { get; } = new (Color.FromRgb(68, 110, 189));
        public bool IsChecked { get; set; }
        public MapUri MapUrl { get; } = new();
        private readonly DroneViewModel _drones;
        private BlApi _bl;

        private Drone? _viewModel;
        public Drone? ViewModel
        {
            get=>_viewModel;
            set
            {
                _viewModel = value;
                OnPropertyChanged();
            }
        }

        private Parcel? _parcel = new();
        public Parcel? Parcel
        {
            get => _parcel;
            set
            {
                _parcel = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Gauge
        public Func<double, string> Formatter { get; set; }
        #endregion

        public DroneTrackingWindow(BlApi bl, Drone drone, DroneViewModel drones)
        {
            Parcel = bl.GetParcels(p => p.Active).FirstOrDefault(p => p.DroneId == drone.Id);
            _bl = bl;
            ViewModel = drone;
            InitializeComponent();
            UpdateContent();
            IconRadioBtn = new IconButton();
            _drones = drones;
            CustomButtons = new WindowControls(this);
            StepProgressBar = new StepProgressBar(Parcel);
        }

        private static Uri NewMapUri(Location location)
        {
            var lat = location.Latitude - location.Latitude % 0.0001;
            var lon = location.Longitude - location.Longitude % 0.0001;

            return new Uri($"https://www.openstreetmap.org/?mlat={lat}&amp;mlon={lon}#map=15/{lat}/{lon}");
        }

        private void UpdateContent()
        {
            ViewModel = _bl.SearchForDrone(d => d.Id == ViewModel.Id);
            Parcel = _bl.GetParcels().FirstOrDefault(p => p.Active && p.DroneId == ViewModel.Id);
            MapUrl.Uri = NewMapUri(_bl.Location(ViewModel));
        }

        private void MouseLeftBtnDown(object sender, MouseButtonEventArgs e) => DragMove();

        internal DroneViewModel GetValue() => _drones;

        private void ChargeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel = _bl.SendDroneToCharge(ViewModel);
                _drones.Update(ViewModel);
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
                ViewModel = _bl.DroneReleaseAndCharge(ViewModel, hours);
                _drones.Update(ViewModel);
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
                ViewModel = _bl.AssignDroneToParcel(ViewModel);
                _drones.Update(ViewModel);
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
                ViewModel = _bl.CollectParcelByDrone(ViewModel);
                _drones.Update(ViewModel);
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
                ViewModel = _bl.DeliverByDrone(ViewModel);
                _drones.Update(ViewModel);
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

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
