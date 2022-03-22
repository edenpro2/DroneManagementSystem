using System;
using System.Xml.Serialization;

namespace DalFacade.DO
{
    [Serializable]
    [XmlRoot]
    public struct Message
    {
        [XmlElement]
        public DateTime sentTime { get; }

        [XmlAttribute]
        private string text { get; }

        [XmlAttribute]
        private int senderId { get; }

        [XmlAttribute]
        private int recipientId { get; }


        private string GetMessage() => text;


        public Message(string msg, int sender, int recipient)
        {
            text = msg;
            sentTime = DateTime.Now;
            senderId = sender;
            recipientId = recipient;
        }

        public override string ToString() => $"At {sentTime}, {senderId} sent {recipientId}: {GetMessage()}";

    }
}
