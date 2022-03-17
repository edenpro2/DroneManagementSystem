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

        public UserViewModel(User user, string phone, string name)
        {
            User = user;
            _phone = phone;
            _name = name;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}