using BLAPI;
using DalFacade.DO;
using PL.ViewModels;
using PL.Windows;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DroneWindow = PL.Windows.Tracking.DroneWindow;

namespace PL.Pages
{
    public partial class DronesPage
    {
        private readonly BlApi _bl;
        public DronesViewModel DronesViewModel { get; private set; }
        public IEnumerable Statuses { get; } = Enum.GetValues(typeof(DroneStatuses));
        public IEnumerable Weights { get; } = Enum.GetValues(typeof(WeightCategories));

        public DronesPage(BlApi ibl)
        {
            _bl = ibl;
            DronesViewModel = new DronesViewModel(_bl.GetDrones(d => d.active));
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
            DronesViewModel.Filtered = DronesViewModel.Drones;

            if (StatusComboBox.SelectedItem != null)
            {
                var cbStatus = (DroneStatuses)StatusComboBox.SelectedItem;
                DronesViewModel.Filtered = new ObservableCollection<Drone>(DronesViewModel.Filtered.Where(drone => drone.status == cbStatus));
            }
            if (WeightComboBox.SelectedItem != null)
            {
                var cbWeight = (WeightCategories)WeightComboBox.SelectedItem;
                DronesViewModel.Filtered = new ObservableCollection<Drone>(DronesViewModel.Filtered.Where(drone => drone.maxWeight == cbWeight));
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
            DronesViewModel.Drones = new ObservableCollection<Drone>(_bl.GetDrones());
            DronesListBox.ItemsSource = DronesViewModel.Drones;
            ClearSelButton_Click(sender, e);
        }

        private void DronesListBox_Click(object sender, MouseButtonEventArgs e)
        {
            var selectedDrone = (Drone)DronesListBox.SelectedItem;

            var droneDetailsWindow =
                new DroneWindow(_bl, selectedDrone, DronesViewModel);

            droneDetailsWindow.ShowDialog();
            DronesViewModel = droneDetailsWindow.GetValue();
            DronesListBox.ItemsSource = DronesViewModel.Drones;
            ClearSelButton_Click(sender, e);
        }


    }
}