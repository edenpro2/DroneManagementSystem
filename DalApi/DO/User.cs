using System;
using System.Xml.Serialization;

namespace DalFacade.DO
{
    [XmlRoot]
    public class User : ViewModelBase
    {
        private int _customerId;
        public int customerId
        {
            get => _customerId;
            set
            {
                if (value == _customerId)
                    return;

                _customerId = value;
                OnPropertyChanged();
            }
        }
        private string _username;
        public string username
        {
            get => _username;
            set
            {
                if (value == _username)
                    return;

                _username = value;
                OnPropertyChanged();
            }
        }
        private string _password;
        public string password
        {
            get => _password;
            set
            {
                if (value == _password)
                    return;

                _password = value;
                OnPropertyChanged();
            }
        }
        private string _email;
        public string email
        {
            get => _email;
            set
            {
                if (value == _email)
                    return;

                _email = value;
                OnPropertyChanged();
            }
        }
        private string _address;
        public string address
        {
            get => _address;
            set
            {
                if (value == _address)
                    return;

                _address = value;
                OnPropertyChanged();
            }
        }
        private string _profilePic;
        public string profilePic
        {
            get => _profilePic;
            set
            {
                if (value == _profilePic)
                    return;

                _profilePic = value;
                OnPropertyChanged();
            }
        }
        private bool _active;
        public bool active
        {
            get => _active;
            set
            {
                if (value == _active)
                    return;

                _active = value;
                OnPropertyChanged();
            }
        }
        private bool _isEmployee;
        public bool isEmployee
        {
            get => _isEmployee;
            set
            {
                if (value == _isEmployee)
                    return;

                _isEmployee = value;
                OnPropertyChanged();
            }
        }

        public User() {}

        public User(int customerId = -1, string user = "", string pass = "", string mail = "", string personalAddress = "", bool employed = false)
        {
            this.customerId = customerId;
            username = user;
            password = pass;
            email = mail;
            address = personalAddress;
            active = true;
            isEmployee = employed;
            profilePic = "../Resources/account.jpg";
        }

        public User(User user)
        {
            customerId = user.customerId;
            username = user.username;
            password = user.password;
            email = user.email;
            address = user.address;
            profilePic = user.profilePic;
            active = user.active;
            isEmployee = user.isEmployee;
        }

        public override string ToString()
        {
            return $"user:{username} \n email:{email} \n id:{customerId}";
        }
    }
}