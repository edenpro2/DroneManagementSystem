using BL.BO;
using BLAPI;
using DalFacade.DO;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace PL.Pages
{
    public partial class SettingsPage
    {
        private readonly BlApi _bl;
        public User User { get; set; }
        public string InfoErrorMessage { get; set; }
        public string BillingErrorMessage { get; set; }
        public SolidColorBrush InfoErrorColor { get; set; }
        public SolidColorBrush BillingErrorColor { get; set; }

        public SettingsPage(BlApi ibl, User user)
        {
            _bl = ibl;
            User = user;
            InitializeComponent();
        }

        private void ApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            InfoErrorMessage = "";
            InfoErrorColor = Brushes.Tomato;
            var name = NameBox.Text;
            var username = UsernameBox.Text;
            var password = PasswordBox.Password;
            var confirmPass = ConfirmPasswordBox.Password;
            var email = EmailBox.Text;
            var confirmEmail = ConfirmEmailBox.Text;

            if (password != confirmPass)
            {
                InfoErrorMessage = "Passwords do not match";
                return;
            }

            var user = User;

            if (password.Length > 0)
            {
                user.Password = password;
            }

            if (email != confirmEmail)
            {
                InfoErrorMessage = "Emails do not match";
                return;
            }

            if (!UserVerification.CheckEmail(email))
            {
                InfoErrorMessage = "Email not valid";
                return;
            }

            // update each field
            user.Username = username;
            user.Email = email;
            User.Customer.Name = name;

            _bl.UpdateCustomer(User.Customer);
            _bl.UpdateUser(user);
            User = user;

            InfoErrorColor = Brushes.Green;
            InfoErrorMessage = "Successfully updated";
        }


        private void UploadPhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                DefaultExt = ".png", // Required file extension 
                Filter = "Image Files|*.jpg;*.jpeg;*.bmp;*.tif;*.png" // Optional
            };

            //To read the content : Get the filename from the OpenFileDialog.

            if (!(bool)fileDialog.ShowDialog())
                return;

            var filePath = fileDialog.FileName;

            if (new FileInfo(filePath).Length > 52_428_800) // > 50 Mb
            {
                InfoErrorMessage = "File too large";
                return;
            }
            var user = User;
            user.ProfilePic = filePath;
            _bl.UpdateUser(user);
            User = user;
            InfoErrorMessage = "";
        }

        private void UpdateBillingBtn_Click(object sender, RoutedEventArgs e)
        {
            BillingErrorMessage = "";
            BillingErrorColor = Brushes.Tomato;

            var user = User;

            var phone = PhoneBox.Text.Where(c => c != ' ').ToArray().ToString();
            var success = long.TryParse(phone, out var phoneNumber);
            var address = AddressBox.Text;

            // update each field
            if (success)
            {
                User.Customer.Phone = phoneNumber.ToString();
            }
            else
            {
                BillingErrorMessage = "Phone is not valid";
                return;
            }

            user.Address = address;

            _bl.UpdateCustomer(User.Customer);
            _bl.UpdateUser(user);
            User = user;
            User.Customer.Phone = phoneNumber.ToString();

            BillingErrorColor = Brushes.Green;
            BillingErrorMessage = "Successfully updated";
        }
    }
}