using DalFacade;
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

            const int STATION_MAX = 5;
            const int DRONE_MAX = 10;
            const int CUSTOMER_MAX = 10;
            const int PARCEL_MAX = 30;

            #endregion

            #region Random intialize loops

            for (var i = 0; i < DRONE_MAX; ++i)
            {
                Drone drone = new(
                    Config.DroneId++,
                    Randomize.Model(rand) ?? string.Empty,
                    (WeightCategories)rand.Next(3));

                // Search for image related to model
                drone.ModelImg = FileReader.GetFilePath(drone.Model, new List<string> { ".png", ".jpg" });
                Drones.Add(drone);
            }

            for (var i = 0; i < STATION_MAX; ++i)
            {
                var location = Randomize.LocationInRadius();

                Stations.Add(new Station(
                    Config.StationId++,
                    rand.Next(MaxStationName),
                    Station.MaxChargeSlots,
                    location.Latitude,
                    location.Longitude));
            }

            for (var i = 0; i < CUSTOMER_MAX - 1; ++i)
            {
                var location = Randomize.LocationInRadius();

                Customers.Add(new Customer(
                    Config.CustomerId++,
                    Randomize.Name(rand),
                    Randomize.Phone(rand),
                    location));
            }

            #endregion

            // Also possible to use Randomize.LocationInRadius() for coordinates

            Customers.Add(new Customer(
                    Config.CustomerId++,
                    "Eden Amiga",
                    "9546582944",
                    new Location(
                        26.03939549441853,
                        -80.29447791353097)
                    )
            );

            string name;

            Users = Customers
                .Where(c => c.Active)
                .Select(c => new User(
                    ref c,
                    name = new string(c.Name.Where(l => !char.IsWhiteSpace(l)).ToArray()) +
                           rand.Next(100),
                    c.Phone,
                    name.ToLower() + "@gmail.com"))
                .ToList();

            #region Admin account
            var eden = Users[Config.CustomerId - 1];
            eden.IsEmployee = true;
            eden.Username = "admin";
            eden.Password = "admin";
            Users[Config.CustomerId - 1] = eden;
            #endregion


            for (var i = 0; i < PARCEL_MAX; ++i)
            {
                DateTime scheduled = default,
                         pickedUp = default,
                         delivered = default,
                         requested = Randomize.Date(rand);

                int senderId = Customers[rand.Next(Customers.Count)].Id;
                int targetId;

                do
                {
                    targetId = Customers[rand.Next(Customers.Count)].Id;
                } while (targetId == senderId);

                Parcels.Add(new Parcel
                (
                    Config.ParcelId++,
                    senderId,
                    targetId,
                    -1,
                    (WeightCategories)rand.Next(3),
                    (Priorities)rand.Next(3),
                    requested,
                    scheduled,
                    pickedUp,
                    delivered
                ));


            }

            #region Chats creator

            var text = File.ReadAllText(FileReader.GetFilePath("RandomText.txt", new List<string> { ".txt" }));

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

                    var name1 = Customers.First(c => c.Id == user1.Customer.Id).Name;
                    var name2 = Customers.First(c => c.Id == user2.Customer.Id).Name;

                    if (j % 2 == 0)
                    {
                        msg = new Message(lines[startPos + j], user1.Customer.Id, user2.Customer.Id, name1, name2);
                    }
                    else
                    {
                        msg = new Message(lines[startPos + j], user2.Customer.Id, user1.Customer.Id, name2, name1);
                    }

                    chat.SendMessage(msg);
                }

                Chats.Add(chat);
            }

            #endregion
        }



    }
}