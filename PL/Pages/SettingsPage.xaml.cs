using BLAPI;
using DalFacade.DO;
using PL.ViewModels;
using System.Windows;

namespace PL.Pages
{
    public partial class SettingsPage
    {
        private readonly BlApi _bl;
        private Customer _customer;
        private UserViewModel ViewModel { get; }

        public SettingsPage(BlApi ibl, User user)
        {
            _bl = ibl;
            _customer = _bl.SearchForCustomer(c => c.id == user.customerId);
            ViewModel = new UserViewModel(user, _customer.phone, _customer.name);
            InitializeComponent();
            DataContext = ViewModel;
        }

        private void ApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            var user = new User(ViewModel.User);

            if (UsernameBox.Text != "")
            {
                user.username = UsernameBox.Text;
            }

            if (PasswordBox.Password != "")
            {
                user.password = PasswordBox.Password;
            }

            if (EmailBox.Text != "")
            {
                user.email = EmailBox.Text;
            }

            if (AddressBox.Text != "")
            {
                user.address = AddressBox.Text;
            }

            if (PhoneBox.Text != "")
            {
                _customer.phone = ViewModel.Phone = PhoneBox.Text;
            }

            ViewModel.User = user;
            _bl.UpdateCustomer(_customer);
            _bl.UpdateUser(user);
            CleanForm();
        }

        private void CleanForm()
        {
            UsernameBox.Text = PasswordBox.Password = EmailBox.Text = AddressBox.Text = PhoneBox.Text = "";
        }
    }
}