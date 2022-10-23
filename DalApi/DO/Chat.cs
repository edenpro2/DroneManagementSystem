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
        [XmlAttribute(DataType = "date")]
        public DateTime createdOn { get; set; }

        [XmlElement]
        public User user1 { get; set; }

        [XmlElement]
        public User user2 { get; set; }

        [XmlArray]
        [XmlArrayItem("Message")]
        public List<Message> messages { get; set; }

        public Message LastMessage => GetLastMessage();

        public Chat()
        {
            createdOn = default;
            user1 = default;
            user2 = default;
            messages = new List<Message>();
        }

        public Chat(User u1, User u2)
        {
            createdOn = DateTime.Now;
            user1 = u1;
            user2 = u2;
            messages = new List<Message>();
        }

        public void SendMessage(Message msg)
        {
            messages.Add(msg);
        }

        public Message GetLastMessage()
        {
            return messages.Last();
        }

        public override string ToString()
        {
            return $"Created on {createdOn}, between {user1.Username} and {user2.Username}";
        }
    }
}
