using System;
using System.Xml.Serialization;

namespace DalFacade.DO
{
    [Serializable]
    [XmlRoot]
    public struct Message
    {
        [XmlElement(DataType = "date")]
        public DateTime sentTime { get; set; }

        [XmlAttribute]
        public string text { get; set; }

        [XmlAttribute]
        public int senderId { get; set; }

        [XmlAttribute]
        public int recipientId { get; set; }

        [XmlAttribute]
        public string senderName { get; set; }

        [XmlAttribute]
        public string recipientName { get; set; }

        private string GetMessage() => text;


        public Message(string msg, int sender, int recipient, string sName, string rName)
        {
            text = msg;
            sentTime = DateTime.Now;
            senderId = sender;
            recipientId = recipient;
            senderName = sName;
            recipientName = rName;
        }

        public override string ToString() => $"At {sentTime}, {senderId} sent {recipientId}: {GetMessage()}";

    }
}
