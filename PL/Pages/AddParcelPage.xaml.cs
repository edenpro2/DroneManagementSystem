using BLAPI;
using DalFacade.DO;
using System;
using System.Linq;
using System.Windows;

namespace PL.Pages
{
    public partial class AddParcelPage
    {
        private readonly BlApi _bl;
        private readonly User _user;
        public string personalId { get; }
        public string currentTime { get; private set; } = DateTime.Now.ToShortDateString();
        public string total { get; } = $"$ {new Random().Next(500)}";

        public AddParcelPage(BlApi ibl, User user)
        {
            _bl = ibl;
            _user = user;
            InitializeComponent();
            personalId = $"{(uint)_user.username.GetHashCode()}";
            DataContext = this;

            NameComboBox.ItemsSource =
                from customer in _bl.GetCustomers()
                where customer.active
                where customer.id != user.customerId
                select customer.name;
        }

        private void ContinueBtn_Click(object sender, RoutedEventArgs e)
        {
            if (NameComboBox.SelectedItem == null)
            {
                ErrorTextBlock.Text = "Please select a recipient";
            }
            else if (WeightCheckBox1.IsChecked is false or null && WeightCheckBox2.IsChecked is false or null && WeightCheckBox3.IsChecked is false or null)
            {
                ErrorTextBlock.Text = "Please select parcel weight";
            }
            else if (PriorityCheckBox1.IsChecked is false or null && PriorityCheckBox2.IsChecked is false or null && PriorityCheckBox3.IsChecked is false or null)
            {
                ErrorTextBlock.Text = "Please select parcel priority";
            }
            else
            {
                ErrorTextBlock.Text = "";
                var targetId = _bl.SearchForCustomer(c => c.name == (string)NameComboBox.SelectedItem).id;

                WeightCategories weight;
                Priorities priority;

                if (WeightCheckBox1.IsChecked == true)
                {
                    weight = WeightCategories.Light;
                }
                else if (WeightCheckBox2.IsChecked == true)
                {
                    weight = WeightCategories.Medium;
                }
                else
                {
                    weight = WeightCategories.Heavy;
                }

                if (PriorityCheckBox1.IsChecked == true)
                {
                    priority = Priorities.Regular;
                }
                else if (PriorityCheckBox2.IsChecked == true)
                {
                    priority = Priorities.Fast;
                }
                else
                {
                    priority = Priorities.Emergency;
                }

                _bl.CreateParcel(_user.customerId, targetId, weight, priority);

                NameComboBox.SelectedItem = null;
                currentTime = DateTime.Now.ToShortDateString();
                WeightCheckBox1.IsChecked = WeightCheckBox2.IsChecked = WeightCheckBox3.IsChecked = false;
                PriorityCheckBox1.IsChecked = PriorityCheckBox2.IsChecked = PriorityCheckBox3.IsChecked = false;
            }
        }
    }
}