using DalFacade.DO;
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
            return DalApi.GetStations(predicate);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> predicate)
        {
            return DalApi.GetCustomers(predicate);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate)
        {
            return DalApi.GetParcels(predicate);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetUsers(Predicate<User> predicate)
        {
            return DalApi.GetUsers(predicate);
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
            return DalApi.GetStations();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomers()
        {
            return DalApi.GetCustomers();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels()
        {
            return DalApi.GetParcels();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetUsers()
        {
            return DalApi.GetUsers();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetEmployees()
        {
            return DalApi.GetEmployees();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Chat> GetChats()
        {
            return DalApi.GetChats();
        }
        #endregion
    }
}