using DalFacade.DO;
using System.Runtime.CompilerServices;

namespace BL
{
    public partial class Bl
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(Drone drone)
        {
            drone.active = false;
            UpdateDrone(drone);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(Station station)
        {
            station.active = false;
            UpdateStation(station);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteCustomer(Customer customer)
        {
            customer.active = false;
            UpdateCustomer(customer);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(Parcel parcel)
        {
            parcel.active = false;
            UpdateParcel(parcel);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteUser(User user)
        {
            user.active = false;
            UpdateUser(user);
        }
    }
}