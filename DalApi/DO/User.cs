using System.Xml.Serialization;

namespace DalFacade.DO
{
    [XmlRoot]
    public struct User
    {
        [XmlAttribute]
        public int customerId { get; set; }
        [XmlAttribute]
        public string username { get; set; }
        [XmlAttribute]
        public string password { get; set; }
        [XmlAttribute]
        public string email { get; set; }
        [XmlAttribute]
        public string address { get; set; }
        [XmlAttribute]
        public string profilePic { get; set; }
        [XmlAttribute]
        public bool active { get; set; }
        [XmlAttribute]
        public bool isEmployee { get; set; }

        public User(int customerId = -1, string user = "", string pass = "", string mail = "",
            string personalAddress = "", bool employed = false)
        {
            this.customerId = customerId;
            username = user;
            password = pass;
            email = mail;
            address = personalAddress;
            active = true;
            isEmployee = employed;
            profilePic = "../Resources/account.jpg";
        }

        public User(User user)
        {
            customerId = user.customerId;
            username = user.username;
            password = user.password;
            email = user.email;
            address = user.address;
            profilePic = user.profilePic;
            active = user.active;
            isEmployee = user.isEmployee;
        }

    }
}