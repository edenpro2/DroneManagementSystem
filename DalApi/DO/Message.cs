using System;
using System.Xml.Serialization;

namespace DalFacade.DO
{
    [XmlRoot]
    public class Message : ViewModelBase
    {
        private DateTime _sentTime;
        [XmlElement(DataType = "date")]
        public DateTime SentTime
        {
            get => _sentTime;
            set
            {
                _sentTime = value;
                OnPropertyChanged();
            }
        }

        private string _text;
        [XmlAttribute]
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        private int _senderId;
        [XmlAttribute]
        public int SenderId
        {
            get => _senderId;
            set
            {
                _senderId = value;
                OnPropertyChanged();
            }
        }

        private int _recipientId;
        [XmlAttribute]
        public int RecipientId
        {
            get => _recipientId;
            set
            {
                _recipientId = value;
                OnPropertyChanged();
            }
        }

        private string _senderName;
        [XmlAttribute]
        public string SenderName
        {
            get => _senderName;
            set
            {
                _senderName = value;
                OnPropertyChanged();
            }
        }

        private string _recipientName;
        [XmlAttribute]
        public string RecipientName
        {
            get => _recipientName;
            set
            {
                _recipientName = value;
                OnPropertyChanged();
            }
        }

        public Message() { }

        public Message(string msg = "", int sender = -1, int recipient = -1, string sName = "", string rName = "")
        {
            Text = msg;
            SentTime = DateTime.Now;
            SenderId = sender;
            RecipientId = recipient;
            SenderName = sName;
            RecipientName = rName;
        }

        public override string ToString()
        {
            return $"At {SentTime}, {SenderId} sent {RecipientId}: {Text}";
        }
    }
}
