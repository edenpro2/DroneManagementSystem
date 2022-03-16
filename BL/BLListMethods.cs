using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BL
{
    public partial class Bl
    {
        #region Filtered lists
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDrones(Predicate<Drone> predicate)
        {
            return _drones.FindAll(predicate).Select(d => d);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStations(Predicate<Station> predicate)
        {
            return dalApi.GetStations(predicate);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> predicate)
        {
            return dalApi.GetCustomers(predicate);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate)
        {
            return dalApi.GetParcels(predicate);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetUsers(Predicate<User> predicate)
        {
            return dalApi.GetUsers(predicate);
        }
        #endregion

        #region Default lists
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDrones()
        {
            return _drones;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStations()
        {
            return dalApi.GetStations();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomers()
        {
            return dalApi.GetCustomers();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels()
        {
            return dalApi.GetParcels();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetUsers()
        {
            return dalApi.GetUsers();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetEmployees()
        {
            return dalApi.GetEmployees();
        }
        #endregion
    }
}