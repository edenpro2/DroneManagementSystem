using DalFacade.DO;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PL.ViewModels
{
    public sealed class ChatViewModel : INotifyPropertyChanged
    {

        private Chat _chat;

        public Chat Chat
        {
            get => _chat;
            set
            {
                _chat = value;
                OnPropertyChanged("_chat");
            }
        }

        private User _receiver;

        public User Receiver
        {
            get => _receiver;
            set
            {
                _receiver = value;
                OnPropertyChanged("_receiver");
            }
        }

        private string _recName;

        public string RecName
        {
            get => _recName;
            set
            {
                _recName = value;
                OnPropertyChanged("_recName");
            }
        }


        public ChatViewModel(Chat chat, User receiver, string rName)
        {
            _chat = chat;
            _receiver = receiver;
            _recName = rName;
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
