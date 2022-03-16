using DO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
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

        #endregion

        internal static List<Drone> Drones = new(MaxDrones);
        internal static List<Station> Stations = new(MaxStations);
        internal static List<Customer> Customers = new(MaxCustomers);
        internal static List<Parcel> Parcels = new(MaxParcels);
        internal static List<User> Users = new(MaxUsers);

        public static void Initialize()
        {
            #region Capacities and Random Seed

            var seed = new Random();
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
                    Randomize.Model(seed),
                    (WeightCategories)seed.Next(3));

                Drones.Add(drone);
            }

            for (var i = 0; i < stationCap; ++i)
            {
                var location = Randomize.LocationInRadius();

                Station station = new(
                    Config.StationId++,
                    seed.Next(MaxStationName),
                    Station.MAXCHARGESLOTS,
                    location.latitude,
                    location.longitude);

                Stations.Add(station);
            }

            for (var i = 0; i < customerCap - 1; ++i)
            {
                var location = Randomize.LocationInRadius();

                Customer customer = new(
                    Config.CustomerId++,
                    Randomize.Name(seed),
                    Randomize.Phone(seed),
                    location.latitude,
                    location.longitude);

                Customers.Add(customer);
            }

            Customers.Add(new Customer(
                    Config.CustomerId++,
                    "Eden Amiga",
                    "9546582943",
                    Randomize.Latitude(seed),
                    Randomize.Longitude(seed))
            );

            string name;

            Users = Customers
                .Where(c => c.active)
                .Select(c => new User(
                    c.id,
                    name = new string(c.name.Where(l => !char.IsWhiteSpace(l)).ToArray()) +
                           seed.Next(100),
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

                var requested = Randomize.Date(seed);
                var senderId = Customers[seed.Next(Customers.Count)].id;
                int targetId;

                do
                {
                    targetId = Customers[seed.Next(Customers.Count)].id;
                } while (targetId == senderId);

                var parcel = new Parcel(
                    Config.ParcelId++,
                    senderId,
                    targetId,
                    -1,
                    (WeightCategories)seed.Next(3),
                    (Priorities)seed.Next(3),
                    requested,
                    scheduled,
                    pickedUp,
                    delivered);

                Parcels.Add(parcel);
            }

            #endregion
        }


    }
}