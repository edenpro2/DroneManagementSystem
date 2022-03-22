using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace DalFacade.DO
{
    [Serializable]
    [XmlRoot]
    public class Chat
    {
        [XmlAttribute]
        public DateTime createdOn { get; }

        [XmlElement]
        public User user1 { get; }

        [XmlElement]
        public User user2 { get; }

        [XmlArray]
        [XmlArrayItem("Message")]
        public List<Message> _messages { get; set; }

        public Message LastMessage => GetLastMessage();

        public Chat()
        {
            createdOn = default;
            user1 = default;
            user2 = default;
            _messages = new List<Message>();
        }

        public Chat(User u1, User u2)
        {
            createdOn = DateTime.Now;
            user1 = u1;
            user2 = u2;
            _messages = new List<Message>();
        }

        public void SendMessage(Message msg) => _messages.Add(msg);

        public Message GetLastMessage() => _messages.Last();

        public override string ToString() => $"Created on {createdOn}, between {user1.username} and {user2.username}";
    }
}
