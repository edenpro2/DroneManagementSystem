using System;
using System.Collections.Generic;
using static System.Math;

namespace DO
{
    public static class Randomize
    {
        #region Names

        public static string[] DroneModels =
        {
            "Silver Arrow Micro-V", "IAI Skylark", "IAI General",
            "IAI Harpy", "IAI I-View", "IAI Panther", "IAI Ranger", "IAI Heron"
        };

        public static string[] CustomerNames =
        {
            "Eden", "Leib", "Aaron", "Shlomo", "David", "Moshe", "Guy", "Kim", "Benny", "Brody", "Matthew",
            "Katie", "Katja", "Emily", "Dwayne", "Gabriella", "Tim", "John", "Sasha", "Anastasia", "Dinn",
            "Olga", "Natasha", "Gustav", "Piet", "Mark", "Vladimir", "Donald", "Joe", "Jill", "Adam", "Luke",
            "Anna", "Peter", "Hans", "Christopher", "Joshua", "Abe", "Jake", "Timothee", "Hanrietta", "Phil"
        };

        public static string[] CustomerLastNames =
        {
            "Cohen", "Levy", "Blam", "Amiga", "Putin", "Trump", "Netanyahu", "Murciano", "Kirshenbaum", "Romanov",
            "Rogers", "Whitefield", "Gruber", "McClane", "Samberg", "Johnson", "Rock", "Rappaport", "Heisenberg",
            "Planck",
            "Armstrong", "Leibowitz", "Adamson", "Lev", "Bar-lev", "Gosalker", "Gonsalez", "Zyrkowsky", "Ten-Boom",
            "Rosenfeld",
            "Ben-nun", "Gomez", "Swift", "Amsalem", "Wick", "Alon", "Oppenheimer", "Einstein", "Heisenberg", "Singh",
            "Kapoor"
        };

        #endregion

        /// <summary>
        /// Chooses a random model name from the list of drone models
        /// </summary>
        /// <param name="rand"> random seed </param>
        /// <returns> model name </returns>
        public static string Model(Random rand)
        {
            return DroneModels[rand.Next(DroneModels.Length)];
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
            var newDate = startDate + newSpan;

            return newDate;
        }

        /// <summary>
        /// Chooses a random customer name and surname
        /// </summary>
        /// <param name="rand"> random seed</param>
        /// <returns> random name and surname</returns>
        public static string Name(Random rand)
        {
            return CustomerNames[rand.Next(CustomerNames.Length)] + " " +
              CustomerLastNames[rand.Next(CustomerLastNames.Length)];
        }

        /// <summary>
        /// Chooses a random phone number of the form 05********
        /// </summary>
        /// <param name="rand">random seed</param>
        /// <returns>random phone number</returns>
        public static string Phone(Random rand)
        {
            return "0" + rand.Next(7) + rand.Next(10000000, 99999999);
        }

        /// <summary>
        /// Returns a random station
        /// </summary>
        /// <param name="stationList"></param>
        /// <param name="rand"></param>
        /// <returns>Random station</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Station Station(List<Station> stationList, Random rand)
        {
            if (stationList == null)
            {
                throw new ArgumentNullException(nameof(stationList));
            }

            return stationList[rand.Next(stationList.Count)];
        }

        /// <summary>
        /// Chooses a random latitude
        /// </summary>
        /// <param name="rand">random seed </param>
        /// <returns>Random degree</returns>
        public static double Latitude(Random rand)
        {
            double[] leftLat =
            {
                29.500245, 31.219613, 31.594679, 31.783575, 32.106105, 32.547652, 32.834150, 32.816392, 33.090650
            };

            double[] rightLat =
            {
                33.317275, 33.028124, 32.746742, 32.640416, 32.262633, 31.964862, 31.731554, 31.731554, 30.834918,
                30.428427, 30.013101, 29.543660
            };

            var first = leftLat[rand.Next(leftLat.Length)];
            var second = rightLat[rand.Next(rightLat.Length)];

            return DoubleBetween(first, second, new Random());
        }

        /// <summary>
        /// Chooses a random longitude
        /// </summary>
        /// <param name="rand"></param>
        /// <returns>Random longitude</returns>
        public static double Longitude(Random rand)
        {
            double[] leftLon =
            {
                34.913901, 34.305699, 34.508550, 34.61944, 34.769092, 34.901029, 34.963355, 35.025942, 35.109777
            };

            double[] rightLon =
            {
                35.770387, 35.848895, 35.752549, 35.573979, 35.497142, 35.480650, 35.480650, 35.502640, 35.502640,
                35.299237, 35.159054, 35.104080, 34.977205
            };

            var first = leftLon[rand.Next(leftLon.Length)];
            var second = rightLon[rand.Next(rightLon.Length)];

            return DoubleBetween(first, second, rand);
        }

        /// <summary>
        /// Gets a double between an interval
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="rand"></param>
        /// <returns></returns>
        private static double DoubleBetween(double minValue, double maxValue, Random rand) => minValue + rand.NextDouble() * (maxValue - minValue);

        /// <summary>
        /// Convert degree to radians
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns>Radians</returns>
        private static double DegreesToRadians(double degrees) => degrees * PI / 180;

        /// <summary>
        /// Convert radians to degrees
        /// </summary>
        /// <param name="radians"></param>
        /// <returns>Degrees</returns>
        private static double RadiansToDegrees(double radians) => radians * 180 / PI;

        /// <summary>
        /// Gets a random location in a given radius from a coordinate
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="radius"></param>
        /// <returns>Location</returns>
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

            var random = new Random();

            var randomLocation = locations[random.Next(locations.Length)];

            const double radiusInMeters = 10000;

            // Convert radius from meters to degrees
            const double radiusInDegrees = radiusInMeters / 111000f;

            var u = random.NextDouble();
            var v = random.NextDouble();
            var w = radiusInDegrees * Sqrt(u);
            var t = 2 * PI * v;
            var x = w * Cos(t);
            var y = w * Sin(t);

            var y0 = randomLocation.latitude;
            var x0 = randomLocation.longitude;

            // Adjust the x-coordinate for the shrinking of the east-west distances
            var newX = x / Cos(DegreesToRadians(y0));

            var foundLongitude = newX + x0;
            var foundLatitude = y + y0;

            return new Location
            {
                latitude = foundLatitude,
                longitude = foundLongitude
            };

        }
    }
}