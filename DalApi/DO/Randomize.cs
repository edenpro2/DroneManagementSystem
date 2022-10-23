#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Math;

namespace DalFacade.DO
{
    public static class Randomize
    {
        #region JsonFiles
        private const string FirstNameJson = "firstnames.json";
        private const string LastNameJson = "lastnames.json";
        #endregion

        #region Lists
        private static readonly IList<string?> ModelNames = FileReader.GetFileNames("Resources\\Models", new List<string> { ".jpg", ".png" }, SearchOption.TopDirectoryOnly);
        private static readonly IList<string> FirstNames = FileReader.LoadJson(FirstNameJson);
        private static readonly IList<string> LastNames = FileReader.LoadJson(LastNameJson);
        #endregion

        /// <summary>
        /// Chooses a random model name from the list of drone models
        /// </summary>
        /// <param name="rand"> random seed </param>
        /// <returns> model name </returns>
        public static string Model(Random rand)
        {
            return ModelNames[rand.Next(ModelNames.Count)];
        }

        /// <summary>
        /// Chooses a random date between 2020 and 2021
        /// </summary>
        /// <param name="rand"> random seed </param>
        /// <returns> random date </returns>
        public static DateTime Date(Random rand)
        {
            var startDate = new DateTime(2021, 1, 1, 0, 0, 0);
            var endDate = DateTime.Now;
            var timeSpan = endDate - startDate;
            var newSpan = new TimeSpan(0, rand.Next(0, (int)timeSpan.TotalMinutes), 0);
            return startDate + newSpan;
        }

        /// <summary>
        /// Chooses a random customer name and surname
        /// </summary>
        /// <param name="rand"> random seed</param>
        /// <returns> random name and surname</returns>
        public static string Name(Random rand)
        {
            return FirstNames.ElementAt(rand.Next(FirstNames.Count)) + " " + LastNames.ElementAt(rand.Next(LastNames.Count));
        }

        /// <summary>
        /// Chooses a random phone number of the form 05********
        /// </summary>
        /// <param name="rand">random seed</param>
        /// <returns>random phone number</returns>
        public static string Phone(Random rand)
        {
            var list = new List<int>
            {
                239, 305, 321, 352, 386, 407, 561, 727, 754, 772, 786, 813, 850, 863, 904, 941, 954
            };
            var areaCodes = list.Select(num => num.ToString()).ToList();

            return areaCodes[rand.Next(areaCodes.Count)] + rand.Next(1000000, 9999999);
        }

        /// <summary>
        /// Returns a random station
        /// </summary>
        /// <param name="stationList"></param>
        /// <param name="rand"></param>
        /// <returns> Random station </returns>
        public static Station Station(IEnumerable<Station> stationList, Random rand)
        {
            return stationList
                .OrderBy(_ => rand.Next())
                .First();
        }

        /// <summary>
        /// Returns a random station
        /// </summary>
        /// <param name="stationList"></param>
        /// <param name="rand"></param>
        /// <returns> Random station </returns>
        public static Station OpenStation(IEnumerable<Station> stationList, Random rand)
        {
            return stationList
                .Where(stn => stn.OpenSlots > 0)
                .OrderBy(_ => rand.Next())
                .First();
        }

        /// <summary>
        /// Get a random status where bound is the max status to choose from (max 3)
        /// </summary>
        /// <example>
        /// GetRandomStatus(rand, 2) => returns free or maintenance
        /// </example>
        /// <param name="rand"></param>
        /// <param name="bound"></param>
        /// <returns></returns>
        public static DroneStatuses GetRandomStatus(Random rand, short bound)
        {
            return (DroneStatuses)rand.Next(bound);
        }

        /// <summary>
        /// Returns a free, active drone
        /// </summary>
        /// <param name="rand"></param>
        /// <param name="parcel"></param>
        /// <returns></returns>
        public static Drone? GetFreeRandomDrone(Random rand, IEnumerable<Drone> drones)
        {
            return drones.
                Where(d => d.Active).
                Where(d => d.Status is DroneStatuses.Free or null).
                OrderBy(_ => rand.Next()).
                FirstOrDefault();
        }

        /// <summary>
        /// Gets a random location in a given radius from a coordinate
        /// </summary>
        /// <returns> Location </returns>
        public static Location LocationInRadius()
        {
            Location[] locations =
            {
                //Gold Coast:
                new(25.53166106, -80.45604003),

                //Kendall:
                new(25.69277059, -80.36444240),

                //Miami:
                new(25.81373808, -80.27764484),

                //North Miami:
                new(25.94322679, -80.23644863),

                //Weston:
                new(26.05098432, -80.34603680),

                //Fort Lauderdale:
                new(26.12437060, -80.20747071),

                //Pompano Beach:
                new(26.27777763, -80.18289037),

                //Boca Raton:
                new(26.44721357, -80.16132918),

                //Palm Springs:
                new(26.64083044, -80.15336961),

                //West Palm Beach:
                new(26.75358149, -80.13730158),

                //Jupiter:
                new(26.91802913, -80.17012176),

                //Palm City:
                new(27.16278540, -80.28451386),

                //Port St. Lucie:
                new(27.25157746, -80.34439087),

                //Vero Beach:
                new(27.57767735, -80.44024867),

                //Melbourne:
                new(28.06798150, -80.70364280),

                //Cocoa West:
                new(28.27874464, -80.80252479),

                //OIA:
                new(28.43609107, -81.41392834),

                //Horizon West (Orlando):
                new(28.43464195, -81.62430062),

                //Orlando:
                new(28.57620066, -81.45003025),

                //Haines City:
                new(28.13521414, -81.58502549),

                //Lakeland:
                new(27.98773187, -82.01222634),

                //Tampa:
                new(28.01913575, -82.48109410),

                //St.Petersburg:
                new(27.84271908, -82.72788128),

                //Spring Hill:
                new(28.41990798, -82.52682799),

                //Port Richey:
                new(28.22890596, -82.65206761),

                //Sarasota:
                new(27.38566918, -82.45871632),

                //Rotonda West:
                new(26.94961686, -82.25271759),

                //Fort Meyers:
                new(26.65408661, -81.97928752),

                //Naples:
                new(26.16924303 - 81.71411233),

                //Bellview:
                new(29.03792121, -82.05059640),

                //Daytona Beach:
                new(29.14568468, -81.08294630),

                //Palm Coast:
                new(29.52782131, -81.26312004),

                //Gainesville:
                new(29.63769446, -82.35970804),

                //St.Augustine:
                new(29.84123950, -81.37503569),

            };

            var random = new Random(Guid.NewGuid().GetHashCode());

            var randomLocation = locations[random.Next(locations.Length)];

            const double radiusInMeters = 5000;

            // Convert radius from meters to degrees
            const double radiusInDegrees = radiusInMeters / 111000f;

            var u = random.NextDouble();
            var v = random.NextDouble();
            var w = radiusInDegrees * Sqrt(u);
            var t = 2 * PI * v;
            var x = w * Cos(t);
            var y = w * Sin(t);

            var y0 = randomLocation.Latitude;
            var x0 = randomLocation.Longitude;

            // Adjust the x-coordinate for the shrinking of the east-west distances
            var newX = x / Cos(y0 * PI / 180);

            var foundLongitude = newX + x0;
            var foundLatitude = y + y0;

            return new(foundLatitude, foundLongitude);

        }
    }
}