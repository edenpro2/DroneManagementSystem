using DalFacade.DO;
using System;
using System.Linq;
using System.Runtime.CompilerServices;


namespace DALXML
{
    public partial class DalXml
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(in Customer customer)
        {
            var customers = GetCustomers().ToList();
            customers.Add(customer);
            Save(CustomersFilePath, customers);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(in Parcel parcel)
        {
            var parcels = GetParcels().ToList();
            parcels.Add(parcel);
            Save(ParcelsFilePath, parcels);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(in Station station)
        {
            var stations = GetStations().ToList();
            stations.Add(station);
            Save(StationsFilePath, stations);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddUser(in User user)
        {
            var users = GetUsers().ToList();
            users.Add(user);
            Save(UsersFilePath, users);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateCustomer(string name, string phone)
        {
            var loc = Randomize.LocationInRadius();
            AddCustomer(new Customer(Config.CustomerId++, name, phone, loc));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateParcel(int senderId, int targetId, WeightCategories weight, Priorities priority)
        {
            AddParcel(new Parcel(Config.ParcelId++, senderId, targetId, -1, weight, priority, DateTime.Now));
        }
    }
}