using BL.BO;
using BLAPI;
using DalFacade.DO;
using Microsoft.Win32;
using PL.ViewModels;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace PL.Pages
{
    public partial class SettingsPage
    {
        private readonly BlApi _bl;
        private Customer _customer;
        public UserViewModel UserViewModel { get; }

        public SettingsPage(BlApi ibl, User user)
        {
            _bl = ibl;
            _customer = _bl.SearchForCustomer(c => c.id == user.customerId);
            UserViewModel = new UserViewModel(user, _customer.phone, _customer.name, user.profilePic);
            InitializeComponent();
        }

        private void ApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            InfoErrorBlock.Text = "";
            InfoErrorBlock.Foreground = Brushes.Tomato;
            var name = NameBox.Text;
            var username = UsernameBox.Text;
            var password = PasswordBox.Password;
            var confirmPass = ConfirmPasswordBox.Password;
            var email = EmailBox.Text;
            var confirmEmail = ConfirmEmailBox.Text;

            if (password != confirmPass)
            {
                InfoErrorBlock.Text = "Passwords do not match";
                return;
            }

            var user = UserViewModel.User;

            if (password.Length > 0)
            {
                user.password = password;
            }

            if (email != confirmEmail)
            {
                InfoErrorBlock.Text = "Emails do not match";
                return;
            }

            if (!UserVerification.CheckEmail(email))
            {
                InfoErrorBlock.Text = "Email not valid";
                return;
            }

            // update each field
            user.username = username;
            user.email = email;
            _customer.name = name;

            _bl.UpdateCustomer(_customer);
            _bl.UpdateUser(user);
            UserViewModel.User = user;
            UserViewModel.Name = name;

            InfoErrorBlock.Foreground = Brushes.Green;
            InfoErrorBlock.Text = "Successfully updated";
        }


        private void UploadPhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                DefaultExt = ".png", // Required file extension 
                Filter = "Image Files|*.jpg;*.jpeg;*.bmp;*.tif;*.png" // Optional
            };

            //To read the content : Get the filename from the OpenFileDialog.

            if ((bool)fileDialog.ShowDialog())
            {
                var filePath = fileDialog.FileName;

                if (new FileInfo(filePath).Length > 52_428_800) // > 50 Mb
                {
                    InfoErrorBlock.Text = "File too large";
                    return;
                }
                var user = UserViewModel.User;
                user.profilePic = filePath;
                UserViewModel.User = user;
                _bl.UpdateUser(user);
                InfoErrorBlock.Text = "";
            }
        }

        private void UpdateBillingBtn_Click(object sender, RoutedEventArgs e)
        {
            BillingErrorBox.Text = "";
            BillingErrorBox.Foreground = Brushes.Tomato;

            var enumerable = PhoneBox.Text.Where(c => c != ' ').ToArray();

            var phone = new string(enumerable);

            var address = AddressBox.Text;
            var user = UserViewModel.User;

            var success = long.TryParse(phone, out var phoneNumber);

            // update each field
            if (success)
            {
                _customer.phone = phoneNumber.ToString();
            }
            else
            {
                BillingErrorBox.Text = "Phone is not valid";
                return;
            }

            user.address = address;

            _bl.UpdateCustomer(_customer);
            _bl.UpdateUser(user);
            UserViewModel.User = user;
            UserViewModel.Phone = phoneNumber.ToString();

            BillingErrorBox.Foreground = Brushes.Green;
            BillingErrorBox.Text = "Successfully updated";
        }
    }
}