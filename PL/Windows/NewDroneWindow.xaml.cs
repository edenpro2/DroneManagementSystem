using BL;
using DalFacade.DO;
using System;
using System.Windows;

namespace PL.Windows
{
    public partial class NewDroneWindow
    {
        private readonly BlApi _bl;

        public NewDroneWindow(BlApi bl)
        {
            _bl = bl;
            InitializeComponent();
            WeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (WeightComboBox.SelectedItem == null)
            {
                ErrorMsg.Text = "Please select max weight";
                return;
            }

            var model = ModelBox.Text;
            var weight = (WeightCategories)WeightComboBox.SelectedItem;

            try
            {
                _bl.CreateDrone(model, weight);
            }
            catch (Exception ex)
            {
                ErrorMsg.Text = ex.Message;
                return;
            }

            Close();
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}