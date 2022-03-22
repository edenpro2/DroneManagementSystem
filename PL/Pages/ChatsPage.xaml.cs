using DalFacade.DO;
using PL.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PL.Pages
{
    public partial class ChatsPage : INotifyPropertyChanged
    {
        public ChatsViewModel chats { get; set; }
        public User user { get; set; }

        public ChatsPage(List<Customer> customers, IEnumerable<Chat> allChats, User user)
        {
            chats = new ChatsViewModel(customers, ref allChats, user);
            this.user = user;
            UserList = allChats.Select(chat =>
            {
                if (chat.user1.Equals(user))
                {
                    return customers.First(c => c.id == chat.user2.customerId).name;
                }

                if (chat.user2.Equals(user))
                {
                    return customers.First(c => c.id == chat.user1.customerId).name;
                }

                return "";

            }).ToList();

            InitializeComponent();
            DataContext = this;
        }

        private string _searchText = "";

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged("_searchText");
                OnPropertyChanged("MyFilteredItems");
            }
        }

        public List<string> UserList { get; set; }

        public IEnumerable<string> MyFilteredItems => UserList.Where(x => x.ToUpper().StartsWith(SearchText.ToUpper()));

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
