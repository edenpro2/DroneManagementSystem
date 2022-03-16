using BLAPI;
using DalFacade.DO;
using PL.ViewModels;
using PL.Windows;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            DataContext = this;
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
                DronesViewModel.Filtered = DronesViewModel.Filtered.Where(drone => drone.status == cbStatus).ToList();
            }
            if (WeightComboBox.SelectedItem != null)
            {
                var cbWeight = (WeightCategories)WeightComboBox.SelectedItem;
                DronesViewModel.Filtered = DronesViewModel.Filtered.Where(drone => drone.maxWeight == cbWeight).ToList();
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
            DronesViewModel.Drones = _bl.GetDrones();
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