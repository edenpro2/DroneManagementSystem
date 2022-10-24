using System.Xml.Serialization;

namespace DalFacade.DO
{
    [XmlRoot]
    public class User : ViewModelBase
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
            Customer = new Customer();
        }

        public override string ToString()
        {
            return $"user:{Username} \n email:{Email} \n id:{Customer.Id}";
        }
    }
}