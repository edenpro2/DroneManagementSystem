using System.Collections.Generic;
using System.ComponentModel;
using BL.BO;
using BL;
using DalFacade.DO;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using DalFacade;

namespace PL.Pages
{
    public partial class SettingsPage : INotifyPropertyChanged
    {
        public sealed class CountryCode
        {
            public string country { get; }
            public string code { get; }
            public string iso { get; } 

            public CountryCode(string country = "", string code = "", string iso = "")
            {
                this.country = country;
                this.code = code;
                this.iso = iso;
            }

            public override string ToString()
            {
                return "+" + code + " " + country;
            }
        }
        private readonly BlApi _bl;
        public User User { get; private set; }

        public List<string> CountryCodes { get; } = FileReader.LoadJson<CountryCode>("countrycodes.json").Select(c => new CountryCode(c.country, c.code).ToString()).ToList();

        private string _infoErrorMessage = "";
        public string InfoErrorMessage
        {
            get => _infoErrorMessage;
            set
            {
                _infoErrorMessage = value;
                OnPropertyChanged();
            }
        }

        private string _billingErrorMessage = "";
        public string BillingErrorMessage
        {
            get => _billingErrorMessage;
            set
            {
                _billingErrorMessage = value;
                OnPropertyChanged();
            }
        } 

        public SettingsPage(BlApi ibl, User user)
        {
            _bl = ibl;
            User = user;
            InitializeComponent();
        }

        private void ApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            InfoErrorMessage = "";
            var name = NameBox.Text;
            var username = UsernameBox.Text;
            var password = PasswordBox.Password;
            var confirmPass = ConfirmPasswordBox.Password;
            var email = EmailBox.Text;

            if (password != confirmPass)
            {
                InfoErrorMessage = "Passwords do not match";
                return;
            }

            var user = new User(User);

            if (password.Length > 0)
            {
                user.Password = password;
            }

            if (!UserVerification.CheckEmail(email))
            {
                InfoErrorMessage = "Email not valid";
                return;
            }

            // update each field
            user.Username = username;
            user.Email = email;
            user.Customer.Name = name;

            if (!User.Equals(user))
            {
                _bl.UpdateCustomer(User.Customer);
                _bl.UpdateUser(user);
                User = user;
                InfoErrorMessage = "Successfully updated";
                return;
            }

            InfoErrorMessage = "Nothing updated";
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

            // Make a copy of user and before copying phone, remove whitespace
            var user = new User(User);
            var phone = PhoneBox.Text = new string(PhoneBox.Text.ToCharArray().Where(char.IsDigit).ToArray());
            var address = AddressBox.Text;

            user.Address = address;
            user.Customer.Phone = phone;

            var c = User.Customer;
            var c1 = user.Customer;

            if (!User.Equals(user))
            {
                _bl.UpdateCustomer(user.Customer);
                _bl.UpdateUser(user);
                User = user;
                InfoErrorMessage = "Successfully updated";
                return;
            }

            BillingErrorMessage = "Nothing updated";
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}