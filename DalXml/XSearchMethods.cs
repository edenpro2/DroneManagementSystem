using DalFacade.DO;
using System;
using System.Linq;
using System.Runtime.CompilerServices;


namespace DALXML
{
    public partial class DalXml
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer SearchForCustomer(Predicate<Customer> predicate)
        {
            return GetCustomers().First(c => predicate(c));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel SearchForParcel(Predicate<Parcel> predicate)
        {
            return GetParcels().First(c => predicate(c));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station SearchForStation(Predicate<Station> predicate)
        {
            return GetStations().First(s => predicate(s));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public User SearchForUser(Predicate<User> predicate)
        {
            return GetUsers().First(u => predicate(u));
        }
    }
}
