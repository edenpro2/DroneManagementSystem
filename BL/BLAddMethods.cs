using DalFacade.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BL
{
    public partial class Bl
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(Drone drone)
        {
            if (_drones.FirstOrDefault(d => d.id == drone.id).Equals(default))
            {
                throw new BlAlreadyExistsException();
            }

            var station = Randomize.Station((List<Station>)GetStations(), new Random());
            AddDrone(drone, station.id);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(Drone drone, int stationId)
        {
            // random battery, status in maintenance and location at the station
            drone.battery = new Random().Next(20, 40);
            drone.status = DroneStatuses.Maintenance;
            var station = SearchForStation(s => s.id == stationId);
            drone.location = new Location(station.latitude, station.longitude);
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