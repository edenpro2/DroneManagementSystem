using BLAPI;
using DalFacade.DO;
using PL.ViewModels;
using PL.Windows;
using PL.Windows.Tracking;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Pages
{
    public partial class DronesPage
    {
        private readonly BlApi _bl;
        public DroneViewModel DroneViewModel { get; private set; }
        public IEnumerable Statuses { get; } = Enum.GetValues(typeof(DroneStatuses));
        public IEnumerable Weights { get; } = Enum.GetValues(typeof(WeightCategories));

        public DronesPage(BlApi ibl)
        {
            _bl = ibl;
            DroneViewModel = new DroneViewModel(_bl.GetDrones(d => d.Active));
            InitializeComponent();
        }

        private void DisplayAsMap_Click(object sender, RoutedEventArgs e)
        {
            new Map(_bl, "drone").Show();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterDrones();
        }

        private void FilterDrones()
        {
            DroneViewModel.Filtered = DroneViewModel.Drones;

            if (StatusComboBox.SelectedItem != null)
            {
                var cbStatus = (DroneStatuses)StatusComboBox.SelectedItem;
                DroneViewModel.Filtered = new ObservableCollection<Drone>(DroneViewModel.Filtered.Where(drone => drone.Status == cbStatus));
            }
            if (WeightComboBox.SelectedItem != null)
            {
                var cbWeight = (WeightCategories)WeightComboBox.SelectedItem;
                DroneViewModel.Filtered = new ObservableCollection<Drone>(DroneViewModel.Filtered.Where(drone => drone.MaxWeight == cbWeight));
            }
        }

        private void ClearSelButton_Click(object sender, RoutedEventArgs e)
        {
            StatusComboBox.SelectedItem = WeightComboBox.SelectedItem = null;
        }

        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            NewDroneWindow newDroneWindow = new(_bl);
            newDroneWindow.ShowDialog();
            DroneViewModel.Drones = new ObservableCollection<Drone>(_bl.GetDrones());
            DronesListBox.ItemsSource = DroneViewModel.Drones;
            ClearSelButton_Click(sender, e);
        }

        private void DronesListBox_Click(object sender, MouseButtonEventArgs e)
        {
            var selectedDrone = (Drone)DronesListBox.SelectedItem;

            var droneDetailsWindow =
                new DroneTrackingWindow(_bl, selectedDrone, DroneViewModel);

            droneDetailsWindow.ShowDialog();
            DroneViewModel = droneDetailsWindow.GetValue();
            DronesListBox.ItemsSource = DroneViewModel.Drones;
            ClearSelButton_Click(sender, e);
        }


    }
}