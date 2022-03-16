using BLAPI;
using DO;
using PL.ViewModels;
using PL.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Pages
{
    public partial class DronesPage
    {
        private readonly BlApi _bl;
        public List<DroneViewModel> viewModel { get; private set; }
        public IEnumerable statuses { get; } = Enum.GetValues(typeof(DroneStatuses));
        public IEnumerable weights { get; } = Enum.GetValues(typeof(WeightCategories));

        public DronesPage(BlApi ibl)
        {
            _bl = ibl;
            viewModel = new List<DroneViewModel>(_bl.GetDrones(d => d.active).Select(d => new DroneViewModel(d)));
            InitializeComponent();
            DataContext = this;
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterDrones();
        }

        private void WeightComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterDrones();
        }

        private void FilterDrones()
        {
            if (StatusComboBox.SelectedItem != null)
            {
                var status = (DroneStatuses)StatusComboBox.SelectedItem;
                viewModel = viewModel.Where(dvm => dvm.Drone.status == status).ToList();
            }

            if (WeightComboBox.SelectedItem != null)
            {
                var weight = (WeightCategories)WeightComboBox.SelectedItem;
                viewModel = viewModel.Where(dvm => dvm.Drone.maxWeight == weight).ToList();
            }
        }

        private void ClearSelButton_Click(object sender, RoutedEventArgs e)
        {
            StatusComboBox.SelectedItem = null;
            WeightComboBox.SelectedItem = null;
        }

        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            NewDroneWindow newDroneWindow = new(_bl);
            newDroneWindow.ShowDialog();
        }

        private void DronesListBox_Click(object sender, MouseButtonEventArgs e)
        {
            if (DronesListBox.SelectedItems.Count != 1)
            {
                return;
            }

            var droneDetailsWindow =
                new DroneWindow(_bl, ((DroneViewModel)DronesListBox.SelectedItem).Drone, viewModel);
            droneDetailsWindow.ShowDialog();
            viewModel = droneDetailsWindow.GetValue();
            DronesListBox.ItemsSource = viewModel;
            ClearSelButton_Click(sender, e);
        }

        private void DisplayAsMap_Click(object sender, RoutedEventArgs e)
        {
            var map = new Map(_bl, "drone");
            map.Show();

        }
    }
}