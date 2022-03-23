using DalFacade.DO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PL.ViewModels
{
    public class ChatsViewModel : INotifyPropertyChanged
    {
        private List<ChatViewModel> _chatViewModels;

        public List<ChatViewModel> ChatViewModels
        {
            get => _chatViewModels;
            set
            {
                _chatViewModels = value;
                OnPropertyChanged("_chatViewModels");
            }
        }

        // Will take in all personalChats and keep only the current user's personalChats
        public ChatsViewModel(IEnumerable<Customer> customerList, ref IEnumerable<Chat> chatList, User user)
        {
            var chats = chatList.Where(chat => chat.user1.customerId == user.customerId || chat.user2.customerId == user.customerId).ToList();
            var customers = customerList.ToList();
            _chatViewModels = new List<ChatViewModel>();

            foreach (var chat in chats)
            {
                string name;

                // if user2 = current sender
                if (chat.user2.customerId == user.customerId)
                {
                    name = customers.First(u => u.id == chat.user1.customerId).name;
                    _chatViewModels.Add(new ChatViewModel(chat, chat.user1, name));
                }
                // if user1 = current sender
                else
                {
                    name = customers.First(u => u.id == chat.user2.customerId).name;
                    _chatViewModels.Add(new ChatViewModel(chat, chat.user2, name));
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
