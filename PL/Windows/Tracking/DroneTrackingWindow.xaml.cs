using BL;
using BL.BO.OSM;
using DalFacade;
using DalFacade.DO;
using PL.Controls;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

#pragma warning disable CS8602 // All null values initialized in constructor

namespace PL.Windows.Tracking
{
    public partial class DroneTrackingWindow
    {
        #region Properties
        private readonly BlApi _bl;
        private string CacheFolder { get; } = FileReader.GetFolderPath("PL\\Cache", FileReader.PathOption.CreateDirectory);

        public BatteryChartPlotter BatteryPlotter { get; } = new();
        public SolidColorBrush CustomFill { get; } = new(Color.FromRgb(68, 110, 189));
        public NominatimJson Details { get; set; }
        public static double MinScreenHeight => PLMethods.MinScreenHeight(0.80);
        public static double MinScreenWidth => PLMethods.MinScreenWidth(0.80);
        public bool IsChecked { get; set; }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            private set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        private string _progressMessage;
        public string ProgressMessage
        {
            get => _progressMessage;
            set
            {
                _progressMessage = value;
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
                var lat = _viewModel.Location.Latitude;
                var lon = _viewModel.Location.Longitude;
                MapUrl = new Uri($"https://www.openstreetmap.org/?mlat={lat}&amp;mlon={lon}#map=12/{lat}/{lon}");
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

        // First, initialize window, then set WebView environment and afterwards assign drone to ViewModel
        // Since SetWebViewEnvironment sets MapView (UIElement) properties, Window must be created beforehand.
        // Afterwards, the ViewModel can be updated (alongside the url)
        public DroneTrackingWindow(BlApi bl, Drone drone)
        {
            InitializeComponent();
            //SetWebViewEnvironment();
            _bl = bl;
            ViewModel = drone;
            UpdateContent();
        }

        #region Async

        //private async void SetWebViewEnvironment()
        //{
        //    var webView2Environment = await CoreWebView2Environment.CreateAsync(null, CacheFolder);
        //    await MapView.EnsureCoreWebView2Async(webView2Environment);
        //}

        #endregion

        private void UpdateContent()
        {
            Parcel = _bl.GetParcels().FirstOrDefault(p => p.Active && p.DroneId == ViewModel.Id);
            BatteryPlotter.Update(_viewModel.Battery);
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
