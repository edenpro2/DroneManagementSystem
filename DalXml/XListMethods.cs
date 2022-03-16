using DalFacade.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;


namespace DALXML
{
    public partial class DalXml
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStations()
        {
            return Load<Station>(StationsFilePath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDrones()
        {
            return Load<Drone>(DronesFilePath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomers()
        {
            return Load<Customer>(CustomersFilePath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels()
        {
            return Load<Parcel>(ParcelsFilePath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetUsers()
        {
            return Load<User>(UsersFilePath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetEmployees()
        {
            return GetUsers().Where(u => u.isEmployee);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStations(Predicate<Station> predicate)
        {
            return GetStations().Where(s => predicate(s));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> predicate)
        {
            return GetCustomers().Where(c => predicate(c));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate)
        {
            return GetParcels().Where(p => predicate(p));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetUsers(Predicate<User> predicate)
        {
            return GetUsers().Where(u => predicate(u));
        }
    }
}