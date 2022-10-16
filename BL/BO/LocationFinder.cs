using DalFacade.DO;
using System.Collections.Generic;
using static BL.BO.GIS;

namespace BL.BO
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
    }
}