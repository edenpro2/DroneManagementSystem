using DalFacade.DO;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DalObject
{
    public partial class DalObject
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStations()
        {
            return new List<Station>(DataSource.Stations);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDrones()
        {
            return new List<Drone>(DataSource.Drones);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomers()
        {
            return new List<Customer>(DataSource.Customers);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels()
        {
            return new List<Parcel>(DataSource.Parcels);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetUsers()
        {
            return new List<User>(DataSource.Users);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Chat> GetChats()
        {
            return new List<Chat>(DataSource.Chats);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetEmployees()
        {
            return DataSource.Users.FindAll(u => u.IsEmployee);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStations(Predicate<Station> predicate)
        {
            return DataSource.Stations.FindAll(predicate);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> predicate)
        {
            return DataSource.Customers.FindAll(predicate);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate)
        {
            return DataSource.Parcels.FindAll(predicate);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetUsers(Predicate<User> predicate)
        {
            return DataSource.Users.FindAll(predicate);
        }
    }
}