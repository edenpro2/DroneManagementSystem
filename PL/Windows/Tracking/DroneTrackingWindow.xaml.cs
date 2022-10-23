using BL;
using BLAPI;
using DalFacade.DO;
using PL.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ScottPlot;

#pragma warning disable CS8602 // All null values initialized in constructor

namespace PL.Windows.Tracking
{
    public partial class DroneTrackingWindow : INotifyPropertyChanged
    {
        #region Properties
        public static double MinScreenHeight { get; } = PLMethods.MinScreenHeight(0.9);
        public static double MinScreenWidth { get; } = PLMethods.MinScreenWidth(0.9);
        public SolidColorBrush CustomFill { get; } = new (Color.FromRgb(68, 110, 189));
        public bool IsChecked { get; set; }
        private readonly DroneViewModel _droneViewModel;
        private readonly BlApi _bl;
        private double[] batteries = new double[1000];
        private double[] seconds = new double[1000];
        private double elapsedTime;
        private const int Delta = 10;

        public WpfPlot BatteryPlot { get; set; } = new();

        private Uri _uri;
        public Uri Uri
        {
            get => _uri;
            set
            {
                _uri = value;
                OnPropertyChanged();
            }
        }

        private Drone _viewModel;
        public Drone ViewModel
        {
            get=>_viewModel;
            set
            {
                _viewModel = value;
                OnPropertyChanged();
                var lat = TruncateLocation(_viewModel.Location.Latitude);
                var lon = TruncateLocation(_viewModel.Location.Longitude);
                Uri = new Uri($"https://www.openstreetmap.org/?mlat={lat}&amp;mlon={lon}#map=12/{lat}/{lon}");
                batteries = batteries.Append(_viewModel.Battery).ToArray();
                seconds = seconds.Append(elapsedTime++).ToArray();
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

        public DroneTrackingWindow(BlApi bl, Drone drone, DroneViewModel droneViewModel)
        {
            _bl = bl;
            ViewModel = drone;
            _droneViewModel = droneViewModel;
            BatteryPlot.Plot.XAxis.Label("Time (seconds)");
            BatteryPlot.Plot.YAxis.Label("Battery (%)");
            BatteryPlot.Plot.Grid(lineStyle: LineStyle.Dot);
            BatteryPlot.Plot.Style(ScottPlot.Style.Seaborn);
            BatteryPlot.Plot.SetAxisLimitsY(0,100);

            InitializeComponent();
            UpdateContent();
        }

        private static double TruncateLocation(double val) => val - val % 0.0001;

        private void UpdateContent()
        {
            Parcel = _bl.GetParcels().FirstOrDefault(p => p.Active && p.DroneId == ViewModel.Id);
            BatteryPlot.Plot.AddScatter(seconds, batteries);
            BatteryPlot.Plot.AddFill(seconds, batteries, 0, System.Drawing.Color.FromArgb(255, 66, 88, 255));
            BatteryPlot.Plot.SetAxisLimitsX(elapsedTime - Delta, elapsedTime + Delta);
            BatteryPlot.Refresh();
        }

        private void MouseLeftBtnDown(object sender, MouseButtonEventArgs e) => DragMove();

        internal DroneViewModel GetValue() => _droneViewModel;

        private void ChargeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel = _bl.SendDroneToCharge(ViewModel);
                _droneViewModel.Update(ViewModel);
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
                _droneViewModel.Update(ViewModel);
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
                _droneViewModel.Update(ViewModel);
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
                _droneViewModel.Update(ViewModel);
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
                _droneViewModel.Update(ViewModel);
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
