using DalFacade.DO;
using System.Runtime.CompilerServices;

namespace BL
{
    public partial class Bl
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(Drone drone)
        {
            drone.Active = false;
            UpdateDrone(drone);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(Station station)
        {
            station.Active = false;
            UpdateStation(station);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteCustomer(Customer customer)
        {
            customer.Active = false;
            UpdateCustomer(customer);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(Parcel parcel)
        {
            parcel.Active = false;
            UpdateParcel(parcel);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteUser(User user)
        {
            user.Active = false;
            UpdateUser(user);
        }
    }
}