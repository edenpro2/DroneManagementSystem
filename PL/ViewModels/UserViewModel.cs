using DO;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PL.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private string name;

        private string phone;

        private User user;

        public User User
        {
            get => user;
            set
            {
                user = value;
                OnPropertyChanged();
            }
        }

        public string Phone
        {
            get => phone;
            set
            {
                phone = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public UserViewModel(User user, string phone, string name)
        {
            User = user;
            this.phone = phone;
            this.name = name;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}