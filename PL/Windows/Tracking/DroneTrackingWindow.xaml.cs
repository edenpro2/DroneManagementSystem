using BL;
using DalFacade.DO;
using PL.Controls;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DalFacade;
using static System.Web.HttpUtility;

#pragma warning disable CS8602 // All null values initialized in constructor

namespace PL.Windows.Tracking
{
    public partial class DroneTrackingWindow
    {
        #region Properties
        public static double MinScreenHeight => PLMethods.MinScreenHeight(0.80);
        public static double MinScreenWidth => PLMethods.MinScreenWidth(0.80);
        public SolidColorBrush CustomFill { get; } = new(Color.FromRgb(68, 110, 189));
        public bool IsChecked { get; set; }
        private readonly BlApi _bl;
        public string ProgressMessage { get; private set; }
        public string ErrorMessage { get; private set; }
        public BatteryChartPlotter BatteryPlotter { get; } = new();

        public FileReader.NominatimJson Details { get; set; }

        private Uri _addressUrl;
        public Uri AddressUrl
        {
            get => _addressUrl;
            set
            {
                _addressUrl = value;
                OnPropertyChanged();
            }
        }

        private Uri _mapUrl;
        public Uri MapUrl
        {
            get => _mapUrl;
            set
            {
                _mapUrl = value;
                OnPropertyChanged();
            }
        }

        private Drone _viewModel;
        public Drone ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                OnPropertyChanged();
                //var lat = TruncateLocation(_viewModel.Location.Latitude);
                //var lon = TruncateLocation(_viewModel.Location.Longitude);
                //MapUrl = new Uri($"https://www.openstreetmap.org/?mlat={lat}&amp;mlon={lon}#map=12/{lat}/{lon}");
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

        public DroneTrackingWindow(BlApi bl, Drone drone)
        {
            _bl = bl;
            ViewModel = drone;
            InitializeComponent();
            UpdateContent();
        }

        private static double TruncateLocation(double val) => val - val % 0.0001;

        private void UpdateContent()
        {
            Parcel = _bl.GetParcels().FirstOrDefault(p => p.Active && p.DroneId == ViewModel.Id);
            BatteryPlotter.Update(_viewModel.Battery);
            GetQuery(ViewModel.Location);
            ErrorMessage = "";
        }

        private void MouseLeftBtnDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void ChargeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _bl.SendDroneToCharge(ViewModel);
                UpdateContent();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
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
                _bl.DroneReleaseAndCharge(ViewModel, hours);
                UpdateContent();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private void AssignBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _bl.AssignDroneToParcel(ViewModel);
                UpdateContent();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private void CollectBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _bl.CollectParcelByDrone(ViewModel);
                UpdateContent();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private void DeliverButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _bl.DeliverByDrone(ViewModel);
                UpdateContent();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
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
