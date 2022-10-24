using BL;
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
            new Map(_bl, nameof(Drone)).Show();
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

            if (WeightComboBox.SelectedItem == null)
                return;

            var cbWeight = (WeightCategories)WeightComboBox.SelectedItem;
            DroneViewModel.Filtered = new ObservableCollection<Drone>(DroneViewModel.Filtered.Where(drone => drone.MaxWeight == cbWeight));

        }

        private void ClearSelButton_Click(object sender, RoutedEventArgs e)
        {
            StatusComboBox.SelectedItem = WeightComboBox.SelectedItem = null;
        }

        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new NewDroneWindow(_bl).ShowDialog();
            DroneViewModel.Drones = new ObservableCollection<Drone>(_bl.GetDrones());
            DronesListBox.ItemsSource = DroneViewModel.Drones;
            ClearSelButton_Click(sender, e);
        }

        private void DronesListBox_Click(object sender, MouseButtonEventArgs e)
        {
            new DroneTrackingWindow(_bl, (Drone)DronesListBox.SelectedItem).ShowDialog();
            FilterDrones();
        }


    }
}