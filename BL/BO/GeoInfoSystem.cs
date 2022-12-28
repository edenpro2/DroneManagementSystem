using DalFacade.DO;
using System.Linq;
using static System.Math;
using static BL.BO.BlPredicates;

namespace BL.BO
{
    public static class GeoInfoSystem
    {
        /// <summary>
        /// Calculates the distance between two locations in kilometers
        /// </summary>
        /// <param name="loc1"/>
        /// <param name="loc2"/>
        /// <returns>Distance of object at loc1 from loc2</returns>
        public static double Distance(Location loc1, Location loc2)
        {
            var lat1 = loc1.Latitude;
            var lat2 = loc2.Latitude;
            var lon1 = loc1.Longitude;
            var lon2 = loc2.Longitude;

            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            // convert to radians
            lat1 = (lat1) * PI / 180.0;
            lat2 = (lat2) * PI / 180.0;

            // apply formula
            var a = Pow(Sin(dLat / 2), 2) + Pow(Sin(dLon / 2), 2) * Cos(lat1) * Cos(lat2);

            var c = 2 * Asin(Sqrt(a));
            return 6371.0 * c;
        }

        /// <summary>
        /// Returns speed of drone (per simulator second)
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="drone"></param>
        /// <returns></returns>
        public static double Speed(this Bl bl, Drone drone)
        {
            if (drone.Status != DroneStatuses.Delivery)
                return (double)Speeds.Unloaded;

            var parcel = bl.GetParcels(p => p.Active).First(p => p.DroneId == drone.Id); // assigned parcel

            return InTransit(parcel) ? (double)Speeds.Loaded : (double)Speeds.Unloaded;
        }

        /// <summary>
        /// Convert degree to radians
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns>Radians</returns>
        private static double ToRadians(double degrees)
        {
            return degrees * PI / 180;
        }

        /// <summary>
        /// Calculates the bearing (direction) of the drone
        /// </summary>
        /// <param name="loc1"></param>
        /// <param name="loc2"></param>
        /// <returns>Direction of drone</returns>
        private static double Bearing(Location loc1, Location loc2)
        {
            //Convert input values to radians   
            var lat1 = loc1.LatToRadians();
            var long1 = loc1.LonToRadians();
            var lat2 = loc2.LatToRadians();
            var long2 = loc2.LonToRadians();

            var deltaLong = long2 - long1;

            var y = Sin(deltaLong) * Cos(lat2);
            var x = Cos(lat1) * Sin(lat2) - Sin(lat1) * Cos(lat2) * Cos(deltaLong);

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
            var droneStartPoint = bl.LocationOf(drone);
            var distanceCoveredPerHour = bl.Speed(drone); //Distance(droneStartPoint, dest);
            var bearing = Bearing(bl.LocationOf(drone), dest);

            const double radius = 6371.01;
            var distRatio = distanceCoveredPerHour / radius;
            var distRatioSine = Sin(distRatio);
            var distRatioCosine = Cos(distRatio);

            var startLatRad = ToRadians(droneStartPoint.Latitude);
            var startLonRad = ToRadians(droneStartPoint.Longitude);

            var startLatCos = Cos(startLatRad);
            var startLatSin = Sin(startLatRad);

            var endLatRads = Asin((startLatSin * distRatioCosine) + (startLatCos * distRatioSine * Cos(bearing)));

            var endLonRads = startLonRad
                             + Atan2(Sin(bearing) * distRatioSine * startLatCos,
                                 distRatioCosine - startLatSin * Sin(endLatRads));

            // Location in radians
            var newLoc = new Location(endLatRads, endLonRads);

            return new Location(newLoc.LatToDegrees(), newLoc.LonToDegrees());
        }
    }
}