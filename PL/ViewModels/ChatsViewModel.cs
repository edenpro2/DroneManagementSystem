using DalFacade.DO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PL.ViewModels
{
    public class ChatsViewModel : INotifyPropertyChanged
    {
        private List<ChatViewModel> _chats;

        public List<ChatViewModel> Chats
        {
            get => _chats;
            set
            {
                _chats = value;
                OnPropertyChanged("_chats");
            }
        }

        // Will take in all chats and keep only the current user's chats
        public ChatsViewModel(IEnumerable<Customer> customerList, ref IEnumerable<Chat> chatList, User user)
        {
            var chats = chatList.Where(chat => chat.user1.Equals(user) || chat.user2.Equals(user)).ToList();
            var customers = customerList.ToList();
            _chats = new List<ChatViewModel>();

            foreach (var chat in chats)
            {
                string name;

                // if user2 = current sender
                if (chat.user2.Equals(user))
                {
                    name = customers.First(u => u.id == chat.user1.customerId).name;
                    _chats.Add(new ChatViewModel(chat, chat.user1, name));
                }
                else
                {
                    name = customers.First(u => u.id == chat.user2.customerId).name;
                    _chats.Add(new ChatViewModel(chat, chat.user2, name));
                }
            }




        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
