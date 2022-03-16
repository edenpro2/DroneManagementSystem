using System;
using System.Xml.Serialization;

namespace DO
{
    [XmlRoot]
    public struct Parcel
    {
        [XmlAttribute]
        public int id { get; set; }
        [XmlAttribute]
        public int senderId { get; set; }
        [XmlAttribute]
        public int targetId { get; set; }
        [XmlAttribute]
        public int droneId { get; set; }
        [XmlAttribute]
        public WeightCategories weight { get; set; }
        [XmlAttribute]
        public Priorities priority { get; set; }
        [XmlElement]
        public DateTime requested { get; set; }
        [XmlElement]
        public DateTime scheduled { get; set; }
        [XmlElement]
        public DateTime collected { get; set; }
        [XmlElement]
        public DateTime delivered { get; set; }
        [XmlAttribute]
        public bool active { get; set; }


        public Parcel(int id = -1, int senderId = -1, int targetId = -1, int droneId = -1,
            WeightCategories weight = WeightCategories.Light, Priorities priority = Priorities.Regular,
            DateTime requested = default, DateTime scheduled = default, DateTime collected = default,
            DateTime delivered = default) : this()
        {
            this.id = id;
            this.senderId = senderId;
            this.targetId = targetId;
            this.droneId = droneId;
            this.weight = weight;
            this.priority = priority;
            this.requested = requested;
            this.scheduled = scheduled;
            this.collected = collected;
            this.delivered = delivered;
            active = true;
        }

        public override string ToString() =>
            "Id: " + id + '\n' +
            "Sender: " + senderId + '\n' +
            "Target: " + targetId + '\n' +
            "Drone: " + droneId + '\n' +
            "Weight: " + weight + '\n' +
            "Priority: " + priority + '\n' +
            "Requested: " + requested + '\n' +
            "Scheduled: " + scheduled + '\n' +
            "Collected: " + collected + '\n' +
            "Delivered: " + delivered + '\n';
    }
}