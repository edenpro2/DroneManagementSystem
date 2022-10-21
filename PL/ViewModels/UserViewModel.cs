using DalFacade.DO;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PL.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private string _name;

        private string _phone;

        private User _user;

        private string _profilePic;

        public User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string ProfilePic
        {
            get => _profilePic;
            set
            {
                _profilePic = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProfilePic"));
            }
        }

        public UserViewModel(User user, string phone, string name, string pic)
        {
            _user = user;
            _phone = phone;
            _name = name;
            _profilePic = pic;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}