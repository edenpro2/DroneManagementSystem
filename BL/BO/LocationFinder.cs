using BL;
using DO;
using System.Collections.Generic;
using static BO.DistanceFinder;
using static System.Math;

namespace BO
{
    public static class LocationFinder
    {
        /// <summary>
        /// Gets the closest station to the object
        /// </summary>
        /// <paramref name="bl"></paramref>
        /// <paramref name="obj"></paramref>
        /// <returns>The closest station to the given object</returns>
        /// <exception cref="EmptyParameterException"></exception>
        public static Station GetClosestStation(this Bl bl, object obj)
        {
            var stations = bl.GetStations();
            Location location;
            switch (obj)
            {
                case Drone drone:
                    location = bl.Location(drone);
                    return GetClosestStation(stations, location);

                case Customer customer:
                    location = bl.Location(customer);
                    return GetClosestStation(stations, location);

                case Location loc:
                    return GetClosestStation(stations, loc);

                default:
                    throw new EmptyParameterException();
            }
        }

        /// <summary>
        /// Private method of GetClosestStation()
        /// </summary>
        /// <param name="stations"></param>
        /// <param name="location"></param>
        /// <returns>Closest station to object</returns>
        /// <exception cref="EmptyParameterException"></exception>
        private static Station GetClosestStation(IEnumerable<Station> stations, Location location)
        {
            if (stations == null)
            {
                throw new EmptyParameterException();
            }

            var minDistance = 0.0;
            var firstIteration = true;
            var closest = new Station();

            foreach (var station in stations)
            {
                var stationLoc = new Location(station.latitude, station.longitude);
                var current = Distance(stationLoc, location);

                if (current >= minDistance && !firstIteration)
                {
                    continue;
                }

                minDistance = current;
                closest = new Station(station);
                firstIteration = false;
            }

            return closest;
        }

        /// <summary>
        /// Same as GetClosestStation(), but station has to have open slots 
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="drone"></param>
        /// <returns>Closest available station</returns>
        /// <exception cref="EmptyParameterException"></exception>
        public static Station ClosestAvailableStation(this Bl bl, Drone drone)
        {
            var stations = bl.GetStations();

            if (stations == null)
            {
                throw new EmptyParameterException();
            }

            var minDistance = 0.0;
            var firstIteration = true;
            var closest = new Station();
            var objectLoc = bl.Location(drone);

            foreach (var station in stations)
            {
                var stationLoc = new Location(station.latitude, station.longitude);
                var current = Distance(stationLoc, objectLoc);

                if ((current >= minDistance || station.openSlots - 1 < 0) && !firstIteration)
                {
                    continue;
                }

                minDistance = current;
                closest = new Station(station);
                firstIteration = false;
            }

            return closest;
        }

        /// <summary>
        /// Convert degree to radians
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns>Radians</returns>
        private static double DegreesToRadians(double degrees)
        {
            return degrees * PI / 180;
        }

        /// <summary>
        /// Convert radians to degrees
        /// </summary>
        /// <param name="radians"></param>
        /// <returns>Degrees</returns>
        private static double RadiansToDegrees(double radians)
        {
            return radians * 180 / PI;
        }

        /// <summary>
        /// Calculates the bearing (direction) of the drone
        /// </summary>
        /// <param name="loc1"></param>
        /// <param name="loc2"></param>
        /// <returns>Direction of drone</returns>
        private static double Bearing(Location loc1, Location loc2)
        {
            double lat1 = loc1.latitude, long1 = loc1.longitude, lat2 = loc2.latitude, long2 = loc2.longitude;

            //Convert input values to radians   
            lat1 = DegreesToRadians(lat1);
            long1 = DegreesToRadians(long1);
            lat2 = DegreesToRadians(lat2);
            long2 = DegreesToRadians(long2);

            var deltaLong = long2 - long1;

            var y = Sin(deltaLong) * Cos(lat2);
            var x = Cos(lat1) * Sin(lat2) -
                       Sin(lat1) * Cos(lat2) * Cos(deltaLong);

            return Atan2(y, x);
        }

        /// <summary>
        /// Calculates the location of drone given its destination, speed, location and bearing
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="drone"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static Location CalculateLocation(this Bl bl, Drone drone, Location dest)
        {
            var droneStartPoint = bl.Location(drone);
            var distance = bl.Speed(drone); //Distance(droneStartPoint, dest);
            var bearing = Bearing(bl.Location(drone), dest);

            const double radius = 6371.01;
            var distRatio = distance / radius;
            var distRatioSine = Sin(distRatio);
            var distRatioCosine = Cos(distRatio);

            var startLatRad = DegreesToRadians(droneStartPoint.latitude);
            var startLonRad = DegreesToRadians(droneStartPoint.longitude);

            var startLatCos = Cos(startLatRad);
            var startLatSin = Sin(startLatRad);

            var endLatRads = Asin((startLatSin * distRatioCosine) + (startLatCos * distRatioSine * Cos(bearing)));

            var endLonRads = startLonRad
                             + Atan2(
                                 Sin(bearing) * distRatioSine * startLatCos,
                                 distRatioCosine - startLatSin * Sin(endLatRads));

            return new Location
            {
                latitude = RadiansToDegrees(endLatRads),
                longitude = RadiansToDegrees(endLonRads)
            };
        }





    }


}