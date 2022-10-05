using DalFacade.DO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace DalObject
{
    internal static class DataSource
    {
        internal static class Config
        {
            // Consumption is per kilometer
            internal const double ConsumptionWhenFree = 0.1;
            internal const double ConsumptionWhenLight = 0.13;
            internal const double ConsumptionWhenMid = 0.17;
            internal const double ConsumptionWhenHeavy = 0.2;

            // Charge rate per hour
            internal const double ChargingRate = 15.0;

            // Static ID's
            internal static int DroneId;
            internal static int ParcelId;
            internal static int StationId;
            internal static int CustomerId;
        }

        #region Constants

        private const short MaxDrones = 20;
        private const short MaxStations = 10;
        private const short MaxCustomers = 100;
        private const short MaxParcels = 1000;
        private const short MaxUsers = 100;
        private const short MaxStationName = 100;
        private const short MaxChats = 1000;
        #endregion

        internal static List<Drone> Drones = new(MaxDrones);
        internal static List<Station> Stations = new(MaxStations);
        internal static List<Customer> Customers = new(MaxCustomers);
        internal static List<Parcel> Parcels = new(MaxParcels);
        internal static List<Chat> Chats = new(MaxChats);
        internal static List<User> Users = new(MaxUsers);

        public static void Initialize()
        {
            #region Capacities and Random Seed

            // Unpredictable seed
            var cryptoSeed = new RNGCryptoServiceProvider();
            var data = new byte[sizeof(int)];
            cryptoSeed.GetBytes(data);
            var seed = BitConverter.ToInt32(data, 0) & (int.MaxValue - 1);
            var rand = new Random(seed);

            const int stationCap = 5;
            const int droneCap = 10;
            const int customerCap = 10;
            const int parcelCap = 10;

            #endregion

            #region Random intialize loops

            for (var i = 0; i < droneCap; ++i)
            {
                Drone drone = new(
                    Config.DroneId++,
                    Randomize.Model(rand),
                    (WeightCategories)rand.Next(3));

                Drones.Add(drone);
            }

            for (var i = 0; i < stationCap; ++i)
            {
                var location = Randomize.LocationInRadius();

                Station station = new(
                    Config.StationId++,
                    rand.Next(MaxStationName),
                    Station.MaxChargeSlots,
                    location.latitude,
                    location.longitude);

                Stations.Add(station);
            }

            for (var i = 0; i < customerCap - 1; ++i)
            {
                var location = Randomize.LocationInRadius();

                Customer customer = new(
                    Config.CustomerId++,
                    Randomize.Name(rand),
                    Randomize.Phone(rand),
                    location.latitude,
                    location.longitude);

                Customers.Add(customer);
            }

            var edenLoc = Randomize.LocationInRadius();

            Customers.Add(new Customer(
                    Config.CustomerId++,
                    "Eden Amiga",
                    "9546582943",
                    edenLoc.latitude,
                    edenLoc.longitude)
            );

            string name;

            Users = Customers
                .Where(c => c.active)
                .Select(c => new User(
                    c.id,
                    name = new string(c.name.Where(l => !char.IsWhiteSpace(l)).ToArray()) +
                           rand.Next(100),
                    c.phone,
                    name + "@gmail.com"))
                .ToList();

            // This is for me to check the employee login
            var eden = Users[Config.CustomerId - 1];
            eden.isEmployee = true;
            eden.username = "edenpro2";
            eden.password = "eden";
            Users[Config.CustomerId - 1] = eden;


            for (var i = 0; i < parcelCap; ++i)
            {
                DateTime
                    scheduled = default,
                    pickedUp = default,
                    delivered = default;

                var requested = Randomize.Date(rand);
                var senderId = Customers[rand.Next(Customers.Count)].id;
                int targetId;

                do
                {
                    targetId = Customers[rand.Next(Customers.Count)].id;
                } while (targetId == senderId);

                var parcel = new Parcel(
                    Config.ParcelId++,
                    senderId,
                    targetId,
                    -1,
                    (WeightCategories)rand.Next(3),
                    (Priorities)rand.Next(3),
                    requested,
                    scheduled,
                    pickedUp,
                    delivered);

                Parcels.Add(parcel);
            }


            var txtFile = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + @"\RandomText.txt";

            var text = File.ReadAllText(txtFile);

            var lines = text.Split(new[] { "OBI-WAN", "ANAKIN", "COUNT-DOOKU", "PALPATINE", "DROID" },
                    StringSplitOptions.TrimEntries).ToList();

            const int numOfChats = 10;
            var users = Users.ToList();

            for (var i = 0; i < numOfChats; i += 2)
            {
                var chat = new Chat(users[i], users[i + 1]);

                for (var j = 0; j < 15; j++)
                {
                    var startPos = rand.Next(lines.Count() - 16);
                    Message msg;

                    var user1 = users[i];
                    var user2 = users[i + 1];

                    var name1 = Customers.First(c => c.id == user1.customerId).name;
                    var name2 = Customers.First(c => c.id == user2.customerId).name;

                    if (j % 2 == 0)
                    {
                        msg = new Message(lines[startPos + j], user1.customerId, user2.customerId, name1, name2);
                    }
                    else
                    {
                        msg = new Message(lines[startPos + j], user2.customerId, user1.customerId, name2, name1);
                    }

                    chat.SendMessage(msg);
                }

                Chats.Add(chat);
            }

            #endregion
        }



    }
}