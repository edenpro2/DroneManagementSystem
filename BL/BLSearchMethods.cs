using DalFacade.DO;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using static BL.BO.BlPredicates;

namespace BL
{
    public partial class Bl
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone SearchForDrone(Predicate<Drone> predicate)
        {
            return _drones.First(d => predicate(d));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station SearchForStation(Predicate<Station> predicate)
        {
            return DalApi.SearchForStation(predicate);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel SearchForParcel(Predicate<Parcel> predicate)
        {
            return DalApi.SearchForParcel(predicate);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer SearchForCustomer(Predicate<Customer> predicate)
        {
            return DalApi.SearchForCustomer(predicate);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public User SearchForUser(Predicate<User> predicate)
        {
            return DalApi.SearchForUser(predicate);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Location Location(object obj)
        {
            switch (obj)
            {
                case Drone drone:
                    return drone.Location != null ? new Location((Location)drone.Location) : new Location();
                case Station station:
                    return new Location(station.Latitude, station.Longitude);
                case Customer customer:
                    return new Location(customer.Latitude, customer.Longitude);
                // parcel is with sender 
                case Parcel parcel when WaitingForDrone(parcel):
                    return Location(SearchForCustomer(c => c.Id == parcel.SenderId));
                // parcel is with drone
                case Parcel parcel when InTransit(parcel):
                    return Location(SearchForDrone(d => d.Id == parcel.DroneId));
                // parcel is with receiver
                case Parcel parcel when AlreadyDelivered(parcel):
                    return Location(SearchForCustomer(c => c.Id == parcel.TargetId));
                default:
                    return new Location();
            }
        }
    }
}