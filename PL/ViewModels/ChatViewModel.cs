using DalFacade.DO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PL.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private ObservableCollection<Chat> _chatViewModels;
        public ObservableCollection<Chat> ChatViewModels
        {
            get => _chatViewModels;
            set
            {
                _chatViewModels = value;
                OnPropertyChanged();
            }
        }

        // Will take in all personalChats and keep only the current user's personalChats
        public ChatViewModel(IEnumerable<Customer> customerList, ref IEnumerable<Chat> chatList, User user)
        {
            var chats = chatList.Where(chat => chat.user1.Customer.Id == user.Customer.Id || chat.user2.Customer.Id == user.Customer.Id).ToList();
            var customers = customerList.ToList();
            _chatViewModels = new ObservableCollection<Chat>(chats);

            foreach (var chat in chats)
            {
                string name;
                //Todo: Fix name of chat 
                // if user2 = current sender
                if (chat.user2.Customer.Id == user.Customer.Id)
                {
                    name = customers.First(u => u.Id == chat.user1.Customer.Id).Name;
                    //_chatViewModels.Add(new Chat(chat, chat.user1, name));
                }
                // if user1 = current sender
                else
                {
                    name = customers.First(u => u.Id == chat.user2.Customer.Id).Name;
                    //_chatViewModels.Add(new ChatViewModel(chat, chat.user2, name));
                }
            }
        }
    }
}
