using System;
using System.Xml.Serialization;

namespace DalFacade.DO
{
    [XmlRoot]
    public class User : ViewModelBase, IEquatable<User>
    {
        private string _username;
        [XmlAttribute]
        public string Username
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
        [XmlAttribute]
        public string Password
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
        [XmlAttribute]
        public string Email
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
        [XmlAttribute]
        public string Address
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
        [XmlAttribute]
        public string ProfilePic
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
        [XmlAttribute]
        public bool Active
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
        [XmlAttribute]
        public bool IsEmployee
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
        private Customer _customer;
        [XmlElement]
        public Customer Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged();
            }
        }

        public User() { }

        public User(ref Customer customer, string user = "", string pass = "", string mail = "", string personalAddress = "", bool employed = false)
        {
            Customer = customer;
            Username = user;
            Password = pass;
            Email = mail;
            Address = personalAddress;
            Active = true;
            IsEmployee = employed;
            ProfilePic = "../Resources/account.jpg";
        }

        public User(User user)
        {
            Customer = user.Customer;
            Username = user.Username;
            Password = user.Password;
            Email = user.Email;
            Address = user.Address;
            ProfilePic = user.ProfilePic;
            Active = user.Active;
            IsEmployee = user.IsEmployee;
            Customer = new Customer(user.Customer);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && Equals((User)obj);
        }

        public override string ToString()
        {
            return $"user:{Username} \n email:{Email} \n id:{Customer.Id}";
        }

        public bool Equals(User other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return
                _username == other._username &&
                _password == other._password &&
                _email == other._email &&
                _address == other._address &&
                _profilePic == other._profilePic &&
                _active == other._active &&
                _isEmployee == other._isEmployee &&
                _customer.Equals(other._customer);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_username, _password, _email, _address, _profilePic, _active, _isEmployee, _customer);
        }
    }
}