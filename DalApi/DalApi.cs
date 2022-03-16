using DO;
using System;
using System.Collections.Generic;

namespace DALFACADE
{
    public interface DalApi
    {
        // Power consumption method
        public double[] RequestPowerConsumption();

        #region Return-list methods

        /// <summary>Returns a list of stations</summary>
        /// <returns>IEnumerable&lt;Station&gt;</returns>
        public IEnumerable<Station> GetStations();

        /// <summary>Returns a list of drones</summary>
        /// <returns>IEnumerable&lt;Drone&gt;</returns>
        public IEnumerable<Drone> GetDrones();

        /// <summary>Returns a list of customers</summary>
        /// <returns>IEnumerable&lt;Customer&gt;</returns>
        public IEnumerable<Customer> GetCustomers();

        /// <summary>Returns a list of parcels</summary>
        /// <returns>IEnumerable&lt;Parcel&gt;</returns>
        public IEnumerable<Parcel> GetParcels();

        /// <summary>Returns a list of users</summary>
        /// <returns>IEnumerable&lt;User&gt;</returns>
        public IEnumerable<User> GetUsers();

        /// <summary>Returns a list of employees</summary>
        /// <returns>IEnumerable&lt;User&gt;</returns>
        public IEnumerable<User> GetEmployees();

        /// <summary>Returns a list of parcels based on predicate</summary>
        /// <param name="predicate" />
        /// <returns>IEnumerable&lt;Parcel&gt;</returns>
        public IEnumerable<Station> GetStations(Predicate<Station> predicate);

        /// <summary>Returns a list of customers based on predicate</summary>
        /// <param name="predicate" />
        /// <returns>IEnumerable&lt;Customer&gt;</returns>
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> predicate);

        /// <summary>Returns a list of parcels based on predicate</summary>
        /// <param name="predicate" />
        /// <returns>IEnumerable&lt;Parcel&gt;</returns>
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate);

        /// <summary>Returns a list of users based on predicate</summary>
        /// <param name="predicate" />
        /// <returns>IEnumerable&lt;User&gt;</returns>
        public IEnumerable<User> GetUsers(Predicate<User> predicate);

        #endregion

        #region Add-to-list methods

        /// <summary>Adds station to list inside DataSource</summary>
        /// <param name="station" />
        public void AddStation(in Station station);

        /// <summary>Add customer to list inside DataSource</summary>
        /// <param name="customer"></param>
        public void AddCustomer(in Customer customer);

        /// <summary>Add parcel to list inside DataSource</summary>
        /// <param name="parcel"></param>
        public void AddParcel(in Parcel parcel);

        /// <summary>Add user to list inside DataSource</summary>
        /// <param name="user"></param>
        public void AddUser(in User user);

        #endregion

        #region Create methods
        public void CreateCustomer(string name, string phone);
        public void CreateParcel(int senderId, int targetId, WeightCategories weight, Priorities priority);
        #endregion

        #region Delete Methods
        ///// <summary>Deletes station inside DataSource</summary>
        ///// <param name="station"></param>
        //public void DeleteStation(Station station);

        ///// <summary>Deletes customer inside DataSource</summary>
        ///// <param name="customer"></param>
        //public void DeleteCustomer(Customer customer);

        ///// <summary>Deletes parcel inside DataSource</summary>
        ///// <param name="parcel"></param>
        //public void DeleteParcel(Parcel parcel);

        ///// <summary>Deletes user inside DataSource</summary>
        ///// <param name="user"></param>
        //public void DeleteUser(User user);
        #endregion

        #region Search methods

        public Station SearchForStation(Predicate<Station> predicate);
        public Customer SearchForCustomer(Predicate<Customer> predicate);
        public Parcel SearchForParcel(Predicate<Parcel> predicate);
        public User SearchForUser(Predicate<User> predicate);

        #endregion

        #region Update Object Methods
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
    }
}