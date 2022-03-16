using BL;
using DO;
using System.Linq;
using static System.Math;

namespace BO
{
    public static class DistanceFinder
    {
        /// <summary>
        /// Convert degree to radian
        /// </summary>
        /// <param name="degree"></param>
        /// <returns>Radians</returns>
        private static double ToRadians(double degree)
        {
            return degree * (PI / 180.0);
        }

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

    }
}