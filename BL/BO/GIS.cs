using DalFacade.DO;
using System.Linq;
using static System.Math;

namespace BL.BO
{
    public static class GIS
    {
        /// <summary>
        /// Calculates the distance between two locations in kilometers
        /// </summary>
        /// <param name="loc1"/>
        /// <param name="loc2"/>
        /// <returns>Distance of object at loc1 from loc2</returns>
        public static double Distance(Location loc1, Location loc2)
        {
            var lat1 = loc1.latitude;
            var lat2 = loc2.latitude;
            var lon1 = loc1.longitude;
            var lon2 = loc2.longitude;

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
            if (drone.status == DroneStatuses.Delivery)
            {
                var parcel = bl.GetParcels(p => p.active).First(p => p.droneId == drone.id);
                if (parcel.collected != default && parcel.delivered == default)
                {
                    return (double)Speeds.Loaded;
                }
            }

            return (double)Speeds.Unloaded;
        }


        /// <summary>
        /// Convert degree to radians
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns>Radians</returns>
        private static double ToRadians(double degrees) => degrees * PI / 180;

        /// <summary>
        /// Convert radians to degrees
        /// </summary>
        /// <param name="radians"></param>
        /// <returns>Degrees</returns>
        private static double ToDegrees(double radians) => radians * 180 / PI;

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
            lat1 = ToRadians(lat1);
            long1 = ToRadians(long1);
            lat2 = ToRadians(lat2);
            long2 = ToRadians(long2);

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

            var startLatRad = ToRadians(droneStartPoint.latitude);
            var startLonRad = ToRadians(droneStartPoint.longitude);

            var startLatCos = Cos(startLatRad);
            var startLatSin = Sin(startLatRad);

            var endLatRads = Asin((startLatSin * distRatioCosine) + (startLatCos * distRatioSine * Cos(bearing)));

            var endLonRads = startLonRad
                             + Atan2(Sin(bearing) * distRatioSine * startLatCos,
                                 distRatioCosine - startLatSin * Sin(endLatRads));

            return new Location
            {
                latitude = ToDegrees(endLatRads),
                longitude = ToDegrees(endLonRads)
            };
        }
    }
}