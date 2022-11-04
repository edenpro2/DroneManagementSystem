using DalFacade.DO;
using System;
using System.Runtime.CompilerServices;

namespace BL
{
    public sealed partial class Bl
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(Drone drone)
        {
            // random station with open slot
            var station = Randomize.OpenStation(GetStations(), new Random());
            // random battery, status in maintenance and location at the station
            drone.Battery = new Random(Guid.NewGuid().GetHashCode()).Next(20, 40);
            drone.Status = DroneStatuses.Maintenance;
            drone.Location = LocationOf(station);
            _drones.Add(drone);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station station)
        {
            DalApi.AddStation(station);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer customer)
        {
            DalApi.AddCustomer(customer);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(Parcel parcel)
        {
            DalApi.AddParcel(parcel);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddUser(User user)
        {
            DalApi.AddUser(user);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateCustomer(string name, string phone)
        {
            DalApi.CreateCustomer(name, phone);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateParcel(int senderId, int targetId, WeightCategories weight, Priorities priority)
        {
            DalApi.CreateParcel(senderId, targetId, weight, priority);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateDrone(string model, WeightCategories maxWeight)
        {
            var drone = new Drone(_droneId++, model, maxWeight);
            AddDrone(drone);
        }
    }
}