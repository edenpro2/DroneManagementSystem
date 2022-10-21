using DalFacade;
using DalFacade.DO;
using System;
using System.Runtime.CompilerServices;
using static DalObject.DataSource;

namespace DalObject
{
    public partial class DalObject : DalApi
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(in Station station)
        {
            if (Stations.Count + 1 > (short)Maximum.Stations)
                return;

            Stations.Add(station);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(in Customer customer)
        {
            if (Customers.Count + 1 > (short)Maximum.Customers)
                return;

            Customers.Add(customer);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(in Parcel parcel)
        {
            if (Parcels.Count + 1 > (short)Maximum.Packages) 
                return;

            Parcels.Add(parcel);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddUser(in User user)
        {
            Users.Add(user);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateCustomer(string name, string phone)
        {
            var loc = Randomize.LocationInRadius();
            AddCustomer(new Customer(Config.CustomerId++, name, phone, loc.Latitude, loc.Longitude));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateParcel(int senderId, int targetId, WeightCategories weight, Priorities priority)
        {
            AddParcel(new Parcel(Config.ParcelId++, senderId, targetId, -1, weight, priority, DateTime.Now));
        }

    }
}