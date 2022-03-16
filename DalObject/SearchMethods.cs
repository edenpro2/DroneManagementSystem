using DalFacade.DO;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DalObject
{
    public partial class DalObject
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station SearchForStation(Predicate<Station> predicate)
        {
            return DataSource.Stations.First(c => predicate(c));
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer SearchForCustomer(Predicate<Customer> predicate)
        {
            return DataSource.Customers.First(c => predicate(c));
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel SearchForParcel(Predicate<Parcel> predicate)
        {
            return DataSource.Parcels.First(p => predicate(p));
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public User SearchForUser(Predicate<User> predicate)
        {
            return DataSource.Users.First(u => predicate(u));
        }
    }
}