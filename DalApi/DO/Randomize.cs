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
        public static string? Model(Random rand)
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
        /// Gets a random location in a given radius from a coordinate
        /// </summary>
        /// <returns> Location </returns>
        public static Location LocationInRadius()
        {
            var locations = new List<Location>
            {
                //Gold Coast:
                new(25.53166, -80.45604),

                //Kendall:
                new(25.69277, -80.36444),

                //Miami:
                new(25.81373, -80.27764),

                //North Miami:
                new(25.94322, -80.23644),

                //Weston:
                new(26.05098, -80.34603),

                //Fort Lauderdale:
                new(26.12437, -80.20747),

                //Pompano Beach:
                new(26.27777, -80.18289),

                //Boca Raton:
                new(26.44721, -80.16132),

                //Palm Springs:
                new(26.64083, -80.15336),

                //West Palm Beach:
                new(26.75358, -80.13730),

                //Jupiter:
                new(26.91802, -80.17012),

                //Palm City:
                new(27.16278, -80.28451),

                //Port St. Lucie:
                new(27.25157, -80.34439),

                //Vero Beach:
                new(27.57767, -80.44024),

                //Melbourne:
                new(28.06798, -80.70364),

                //Cocoa West:
                new(28.27874, -80.80252),

                //OIA:
                new(28.43609, -81.41392),

                //Horizon West (Orlando):
                new(28.43464, -81.62430),

                //Orlando:
                new(28.57620, -81.45003),

                //Haines City:
                new(28.13521, -81.58502),

                //Lakeland:
                new(27.98773, -82.01222),

                //Tampa:
                new(28.01913, -82.48109),

                //St.Petersburg:
                new(27.84271, -82.72788),

                //Spring Hill:
                new(28.41990, -82.52682),

                //Port Richey:
                new(28.22890, -82.65206),

                //Sarasota:
                new(27.38566, -82.45871),

                //Rotonda West:
                new(26.94961, -82.25271),

                //Fort Meyers:
                new(26.65408, -81.97928),

                //Naples:
                new(26.16924 , -81.71411),

                //Bellview:
                new(29.03792, -82.05059),

                //Daytona Beach:
                new(29.14568, -81.08294),

                //Palm Coast:
                new(29.52782, -81.26312),

                //Gainesville:
                new(29.63769, -82.35970),

                //St.Augustine:
                new(29.84123, -81.37503),

            };

            var random = new Random();

            var randomLocation = locations[random.Next(locations.Count)];
            const int radiusInMeters = 5000;

            // Convert radius from meters to degrees
            const double radiusInDegrees = radiusInMeters / 111_000f;

            var randA = random.NextDouble();
            var randB = random.NextDouble();
            var w = radiusInDegrees * Sqrt(randA);
            var t = 2 * PI * randB;
            var x = w * Cos(t);
            var y = w * Sin(t);

            // Adjust the x-coordinate for the shrinking of the east-west distances
            var newX = x / Cos(randomLocation.Latitude * (PI / 180));


            var lat = Round(y + randomLocation.Latitude, 5);
            var lon = Round(newX + randomLocation.Longitude, 5);

            return new Location(lat, lon);

        }
    }
}