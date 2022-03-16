using DO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;


namespace DALXML
{
    public partial class DalXml
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(Station station)
        {
            var stations = GetStations().ToList();
            stations[station.id] = station;
            UpdateStationList(stations);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(Customer customer)
        {
            var customers = GetCustomers().ToList();
            customers[customer.id] = customer;
            UpdateCustomerList(customers);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcel(Parcel parcel)
        {
            var parcels = GetParcels().ToList();
            parcels[parcel.id] = parcel;
            UpdateParcelList(parcels);

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateUser(User user)
        {
            var users = GetUsers().ToList();
            for (var i = 0; i < users.Capacity; i++)
            {
                if (users[i].customerId == user.customerId)
                {
                    users[i] = user;
                    UpdateUserList(users);
                    return;
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneList(List<Drone> drones)
        {
            Save(DronesFilePath, drones);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStationList(List<Station> stations)
        {
            Save(StationsFilePath, stations);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomerList(List<Customer> customers)
        {
            Save(CustomersFilePath, customers);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcelList(List<Parcel> parcels)
        {
            Save(ParcelsFilePath, parcels);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateUserList(List<User> users)
        {
            Save(UsersFilePath, users);
        }
    }
}
