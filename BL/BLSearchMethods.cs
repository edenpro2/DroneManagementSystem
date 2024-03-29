﻿using DalFacade.DO;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BL
{
    public sealed partial class Bl
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
        public Location LocationOf(object obj)
        {
            switch (obj)
            {
                case Drone drone:
                    return drone.Location;
                case Station station:
                    return station.Location;
                case Customer customer:
                    return customer.Location;
                // parcel is with receiver
                case Parcel parcel when parcel.Delivered != default:
                    return LocationOf(SearchForCustomer(c => c.Id == parcel.TargetId));
                // parcel is with drone
                case Parcel parcel when parcel.Collected != default:
                    return LocationOf(SearchForDrone(d => d.Id == parcel.DroneId));
                // parcel is with sender 
                case Parcel parcel when true:
                    return LocationOf(SearchForCustomer(c => c.Id == parcel.SenderId));
                default:
                    return default;
            }
        }
    }
}