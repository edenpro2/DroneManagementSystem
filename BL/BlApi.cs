using DO;
using System;
using System.Collections.Generic;

namespace BLAPI
{
    public interface BlApi
    {
        #region Add Methods
        public void AddDrone(Drone drone);
        public void AddDrone(Drone drone, int stationId);
        public void AddStation(Station station);
        public void AddCustomer(Customer customer);
        public void AddParcel(Parcel parcel);
        public void AddUser(User user);
        #endregion

        #region Create Methods
        public void CreateCustomer(string name, string phone);
        public void CreateParcel(int senderId, int targetId, WeightCategories weight, Priorities priority);
        public void CreateDrone(string model, WeightCategories maxWeight);
        #endregion

        #region Delete Methods
        public void DeleteDrone(Drone drone);
        public void DeleteStation(Station station);
        public void DeleteCustomer(Customer customer);
        public void DeleteParcel(Parcel parcel);
        public void DeleteUser(User user);
        #endregion

        #region Calculation Methods
        public double CalcPowerConsumption(Drone drone, object o);
        public Location Location(object obj);
        #endregion

        #region List Methods
        public IEnumerable<Drone> GetDrones();
        public IEnumerable<Station> GetStations();
        public IEnumerable<Customer> GetCustomers();
        public IEnumerable<Parcel> GetParcels();
        public IEnumerable<User> GetUsers();
        #endregion

        #region Filtered List Methods
        public IEnumerable<Drone> GetDrones(Predicate<Drone> predicate);
        public IEnumerable<Station> GetStations(Predicate<Station> predicate);
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> predicate);
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate);
        public IEnumerable<User> GetUsers(Predicate<User> predicate);
        #endregion

        #region Search Methods
        public Drone SearchForDrone(Predicate<Drone> predicate);
        public Station SearchForStation(Predicate<Station> predicate);
        public Customer SearchForCustomer(Predicate<Customer> predicate);
        public Parcel SearchForParcel(Predicate<Parcel> predicate);
        public User SearchForUser(Predicate<User> predicate);
        #endregion

        #region Update Class Methods
        public void UpdateDrone(Drone drone);
        public void UpdateStation(Station station);
        public void UpdateCustomer(Customer customer);
        public void UpdateParcel(Parcel parcel);
        public void UpdateUser(User user);
        #endregion

        #region Update List Methods
        public void UpdateDroneList(List<Drone> drones);
        public void UpdateStationList(List<Station> stations);
        public void UpdateCustomerList(List<Customer> customers);
        public void UpdateParcelList(List<Parcel> parcels);
        public void UpdateUserList(List<User> users);
        #endregion

        #region Company Methods
        public Drone SendDroneToCharge(Drone drone);
        public Drone ReceiveDroneForCharging(Drone drone);
        public Drone ChargeDrone(Drone drone, int hours);
        public Drone DroneRelease(Drone drone);
        public Drone DroneReleaseAndCharge(Drone drone, int hours);
        public Drone AssignDroneToParcel(Drone drone);
        public Drone CollectParcelByDrone(Drone drone);
        public Drone DeliverByDrone(Drone drone);
        public Drone UpdateBattery(Drone drone);
        #endregion

        #region Consumption Rates
        public double ConsumptionWhenFree();
        public double ConsumptionWhenLight();
        public double ConsumptionWhenMid();
        public double ConsumptionWhenHeavy();
        #endregion

        #region Simulator Methods
        public Tuple<Drone, string> DroneSimulator(Drone drone);
        #endregion

    }
}