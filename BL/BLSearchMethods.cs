using DO;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using static BO.BlPredicates;

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
            return dalApi.SearchForStation(predicate);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel SearchForParcel(Predicate<Parcel> predicate)
        {
            return dalApi.SearchForParcel(predicate);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer SearchForCustomer(Predicate<Customer> predicate)
        {
            return dalApi.SearchForCustomer(predicate);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public User SearchForUser(Predicate<User> predicate)
        {
            return dalApi.SearchForUser(predicate);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Location Location(object obj)
        {
            switch (obj)
            {
                case Drone drone:
                    return drone.location != null ? new Location((Location)drone.location) : new Location();
                case Station station:
                    return new Location(station.latitude, station.longitude);
                case Customer customer:
                    return new Location(customer.latitude, customer.longitude);
                // parcel is with sender 
                case Parcel parcel when WaitingForDrone(parcel):
                    return Location(SearchForCustomer(c => c.id == parcel.senderId));
                // parcel is with drone
                case Parcel parcel when InTransit(parcel):
                    return Location(SearchForDrone(d => d.id == parcel.droneId));
                // parcel is with receiver
                case Parcel parcel when AlreadyDelivered(parcel):
                    return Location(SearchForCustomer(c => c.id == parcel.targetId));
                default:
                    return new Location();
            }
        }
    }
}