using System.IO;
using BLAPI;
using DalFacade.DO;
using PL.ViewModels;
using System.Windows;
using System.Windows.Media;
using BL.BO;
using Microsoft.Win32;

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
            ErrorBlock.Text = "";
            ErrorBlock.Foreground = Brushes.Tomato;
            var name = NameBox.Text;
            var username = UsernameBox.Text;
            var password = PasswordBox.Password;
            var confirmPass = ConfirmPasswordBox.Password;
            var email = EmailBox.Text;
            var confirmEmail = ConfirmEmailBox.Text;

            if (password != confirmPass)
            {
                ErrorBlock.Text = "Passwords do not match";
                return;
            }

            var user = ViewModel.User;

            if (password.Length > 0)
            {
                user.password = password;
            }

            if (email != confirmEmail)
            {
                ErrorBlock.Text = "Emails do not match";
                return;
            }

            if (!UserVerification.CheckEmail(email))
            {
                ErrorBlock.Text = "Email not valid";
                return;
            }

           

            // update each field
            user.username = username;
            user.email = email;
            ViewModel.Name = name;

            _bl.UpdateCustomer(_customer);
            _bl.UpdateUser(user);
            ViewModel.User = user;

            ErrorBlock.Foreground = Brushes.Green;
            ErrorBlock.Text = "Successfully updated";
        }


        private void UploadPhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                DefaultExt = ".png", // Required file extension 
                Filter = "Image Files|*.jpg;*.jpeg;*.bmp;*.tif;*.png" // Optional
            };

            //To read the content : You will get the filename from the OpenFileDialog and use that to do what IO operation on it.

            if ((bool) fileDialog.ShowDialog())
            {
                var filePath = fileDialog.FileName;

                if (new FileInfo(filePath).Length > 655_360) // > 5 Mb
                {
                    ErrorBlock.Text = "File too large";
                    return;
                }
                var user = ViewModel.User;
                user.profilePic = filePath;
                ViewModel.User = user;
                _bl.UpdateUser(user);
                ErrorBlock.Text = "";
            }
        }

        private void UserInfoBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BillingInfoBtn_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}