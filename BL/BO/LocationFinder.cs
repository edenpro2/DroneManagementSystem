﻿using DalFacade.DO;
using System.Collections.Generic;
using static BL.BO.GeoInfoSystem;

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
                    location = bl.LocationOf(drone);
                    return GetClosestStation(stations, location);

                case Customer customer:
                    location = bl.LocationOf(customer);
                    return GetClosestStation(stations, location);

                case Location loc:
                    return GetClosestStation(stations, loc);

                default:
                    throw new EmptyParameterException(obj.GetType());
            }
        }

        /// <summary>
        /// Private method of GetClosestStation()
        /// </summary>
        /// <param name="stations"></param>
        /// <param name="objectLocation"></param>
        /// <returns>Closest station to object</returns>
        /// <exception cref="EmptyParameterException"></exception>
        private static Station GetClosestStation(IEnumerable<Station> stations, Location objectLocation)
        {
            if (stations == null)
            {
                throw new EmptyParameterException(typeof(IEnumerable<Station>));
            }

            var minDistance = 0.0;
            var firstIteration = true;
            var closest = new Station();

            foreach (var station in stations)
            {
                var current = Distance(station.Location, objectLocation);

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
                throw new EmptyParameterException(typeof(IEnumerable<Station>));
            }

            var minDistance = 0.0;
            var firstIteration = true;
            var closest = new Station();
            var objectLoc = bl.LocationOf(drone);

            foreach (var station in stations)
            {
                var current = Distance(station.Location, objectLoc);

                if ((current >= minDistance || station.OpenSlots - 1 < 0) && !firstIteration)
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