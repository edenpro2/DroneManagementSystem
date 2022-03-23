using System;
using System.Xml.Serialization;

namespace DalFacade.DO
{
    [Serializable]
    [XmlRoot]
    public class User
    {
        public int customerId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string profilePic { get; set; }
        public bool active { get; set; }
        public bool isEmployee { get; set; }

        public User()
        {
            customerId = -1;
            username = "";
            password = "";
            email = "";
            address = "";
            profilePic = "";
            active = false;
            isEmployee = false;
        }

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

        public override string ToString() => $"user:{username} \n email:{email} \n id:{customerId}";

    }
}