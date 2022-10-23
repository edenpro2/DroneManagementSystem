#nullable enable
using DalFacade.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static BL.BO.BlPredicates;
using static BL.BO.GIS;
using static BL.BO.LocationFinder;
using static DalFacade.DO.DroneStatuses;

namespace BL
{
    public partial class Bl
    {
        #region Update Individual Objects
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(Drone drone)
        {
            var drones = GetDrones().ToList();

            for (var i = 0; i < drones.Count; i++)
            {
                if (drones[i].Id == drone.Id)
                {
                    drones[i] = drone;
                    UpdateDroneList(drones);
                    break;
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(Station station)
        {
            DalApi.UpdateStation(station);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(Customer customer)
        {
            DalApi.UpdateCustomer(customer);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcel(Parcel parcel)
        {
            DalApi.UpdateParcel(parcel);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateUser(User user)
        {
            DalApi.UpdateUser(user);
        }

        #endregion

        #region Update Entire Lists
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneList(List<Drone> drones)
        {
            _drones = drones;
            DalApi.UpdateDroneList(drones);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStationList(List<Station> stations)
        {
            DalApi.UpdateStationList(stations);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomerList(List<Customer> customers)
        {
            DalApi.UpdateCustomerList(customers);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcelList(List<Parcel> parcels)
        {
            DalApi.UpdateParcelList(parcels);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateUserList(List<User> users)
        {
            DalApi.UpdateUserList(users);
        }
        #endregion

        #region Drone updates

        /// <summary>
        /// Drone is added to ports in station (location = station)
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        /// <exception cref="BlDroneNotMaintainedException"></exception>
        /// <exception cref="BlNoOpenSlotsException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone ReceiveDroneForCharging(Drone drone)
        {
            if (drone.Status is not Maintenance)
                throw new BlDroneNotMaintainedException();

            var closestStation = this.ClosestAvailableStation(drone);

            if (closestStation.Equals(default))
                throw new BlNoOpenSlotsException();

            var closestLocation = LocationOf(closestStation);

            lock (DalApi)
            {
                closestStation.OpenSlots--;
                closestStation.Ports.Add(new DroneCharge(closestStation.Id, drone.Id));
                UpdateStation(closestStation);

                drone.Location = closestLocation;
                UpdateDrone(drone);

                return drone;
            }
        }

        /// <summary>
        /// Drone is sent to charge at closest station (location unchanged)
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        /// <exception cref="BlDroneNotFreeException"></exception>
        /// <exception cref="BlNoOpenSlotsException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone SendDroneToCharge(Drone drone)
        {
            if (drone.Status is not Free)
                throw new BlDroneNotFreeException();

            var closestStation = this.ClosestAvailableStation(drone);

            if (closestStation.Equals(default))
                throw new BlNoOpenSlotsException();

            lock (DalApi)
            {
                closestStation.OpenSlots--;
                closestStation.Ports.Add(new DroneCharge(closestStation.Id, drone.Id));
                UpdateStation(closestStation);

                drone.Status = Maintenance;
                UpdateDrone(drone);

                return drone;
            }
        }

        /// <summary>
        /// Charge drone for x number of hours (location unchanged)
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public Drone ChargeDrone(Drone drone, int hours)
        {
            var total = _droneChargeRate * hours;

            if (drone.Battery + total > 100)
                drone.Battery = 100;
            else
                drone.Battery += total;

            UpdateDrone(drone);
            return drone;
        }

        /// <summary>
        /// Charges drone for x hours and releases it (location unchanged). This is for non-threaded simulation
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        /// <exception cref="BlDroneNotMaintainedException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone DroneReleaseAndCharge(Drone drone, int hours)
        {
            if (drone.Status is not Maintenance)
                throw new BlDroneNotMaintainedException();

            var droneLocation = LocationOf(drone);

            lock (DalApi)
            {
                var batteryAfterCharge = _droneChargeRate * hours;

                if (drone.Battery + batteryAfterCharge > 100)
                    drone.Battery = 100;
                else
                    drone.Battery += batteryAfterCharge;

                drone.Status = Free;

                UpdateDrone(drone);

                // Get station with same location as drone
                var station = GetStations().First(s => LocationOf(s).Equals(droneLocation));
                station.OpenSlots++;
                station.Ports.Remove(station.Ports.Find(p => p.DroneId == drone.Id));
                UpdateStation(station);

                return drone;
            }
        }

        /// <summary>
        /// Release drone from charging and delete port (location unchanged)
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        /// <exception cref="BlDroneNotMaintainedException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone DroneRelease(Drone drone)
        {
            if (drone.Status is not Maintenance)
                throw new BlDroneNotMaintainedException();

            lock (DalApi)
            {
                drone.Status = Free;
                UpdateDrone(drone);

                // Delete droneCharge
                var station = this.GetClosestStation(drone);
                if (station.Ports.Exists(p => p.DroneId == drone.Id))
                {
                    station.OpenSlots++;
                    station.Ports.Remove(station.Ports.First(p => p.DroneId == drone.Id));
                    UpdateStation(station);
                }

                return drone;
            }
        }

        /// <summary>
        /// Drone is assigned to best matching parcel (location unchanged)
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        /// <exception cref="BlDroneNotFreeException"></exception>
        /// <exception cref="BlNoMatchingParcels"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone AssignDroneToParcel(Drone drone)
        {
            // can't assign if not free
            if (drone.Status is not Free)
                throw new BlDroneNotFreeException();

            // look for the highest priority parcel and closest available drone 
            var parcel = BestMatchingParcel(drone);

            // if parcel not found will be automatically default
            if (parcel is null || parcel.Active is false)
                throw new BlNoMatchingParcels();

            lock (DalApi)
            {
                // update both parcel and drone
                parcel.DroneId = drone.Id;
                parcel.Scheduled = DateTime.Now;
                UpdateParcel(parcel);
                drone.Status = Delivery;
                UpdateDrone(drone);

                return drone;
            }
        }

        /// <summary>
        /// Collects the parcel and (location = parcel)
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        /// <exception cref="BlNotFoundException"></exception>
        /// <exception cref="BlAlreadyCollected"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone CollectParcelByDrone(Drone drone)
        {
            var parcel = GetParcels(p => p.Active).FirstOrDefault(p => p.DroneId == drone.Id);

            if (parcel is null || parcel.Requested == default)
                throw new BlNotFoundException();

            if (!WaitingForDrone(parcel))
                throw new BlAlreadyCollected();

            lock (DalApi)
            {
                var parcelLocation = LocationOf(parcel);

                drone.Location = parcelLocation;
                UpdateDrone(drone);

                parcel.Collected = DateTime.Now;
                UpdateParcel(parcel);

                return drone;
            }
        }

        /// <summary>
        /// Delivers parcel to addressee (location = target)
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        /// <exception cref="BlNotBeingDeliveredException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone DeliverByDrone(Drone drone)
        {
            var parcel = GetParcels(p => p.Active).FirstOrDefault(p => p.DroneId == drone.Id);

            if (parcel is null || parcel.Delivered != default || parcel.Active == false)
            {
                throw new BlNotBeingDeliveredException();
            }

            lock (DalApi)
            {
                drone.Status = Free;
                drone.Location = LocationOf(SearchForCustomer(c => c.Id == parcel.TargetId));
                UpdateDrone(drone);

                parcel.Delivered = DateTime.Now;
                parcel.Active = false;
                UpdateParcel(parcel);

                return drone;
            }
        }

        /// <summary>
        /// Either reduces battery by consumption or charges it according to charge rate
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone UpdateBattery(Drone drone)
        {
            double change;
            var id = drone.Id;
            switch (drone.Status)
            {
                case Delivery:
                    var parcel = GetParcels(p => p.Active).First(p => p.DroneId == id);

                    // drone hasn't collected parcel yet
                    if (WaitingForDrone(parcel) || NotAssignedToDrone(parcel))
                    {
                        change = - ConsumptionWhenFree();
                    }

                    // drone is delivering parcel
                    else
                    {
                        switch (parcel.Weight)
                        {
                            case WeightCategories.Light:
                                change = - ConsumptionWhenLight();
                                break;
                            case WeightCategories.Medium:
                                change = - ConsumptionWhenMid();
                                break;
                            case WeightCategories.Heavy:
                                change = - ConsumptionWhenHeavy();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    break;
                case Maintenance:
                    // drone is at the station (negate change)
                    if (LocationOf(drone).Equals(LocationOf(this.GetClosestStation(drone))))
                    {
                        change = _droneChargeRate;

                        // if overflow detected, change is adjusted
                        if (drone.Battery + change > 100)
                        {
                            change = 100 - drone.Battery;
                        }
                    }
                    // drone is on it's way to station (hence it's free)
                    else
                    {
                        change = - ConsumptionWhenFree();
                    }

                    break;
                case Free:
                    // drone is at station (idle state)
                    if (LocationOf(drone).Equals(LocationOf(this.GetClosestStation(drone))))
                    {
                        change = 0;
                    }
                    // drone is on it's way to station (and free)
                    else
                    {
                        change = - ConsumptionWhenFree();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            drone.Battery += change;
            return drone;
        }

        private Parcel BestMatchingParcel(Drone drone)
        {
            lock (DalApi)
            {
                return
                    GetParcels(p => p.Active)
                    .Where(p => NotAssignedToDrone(p))
                    .Where(p => p.Weight <= drone.MaxWeight)
                    .OrderByDescending(p => p.Priority)
                    .ThenByDescending(p => p.Weight)
                    .ThenBy(p => Distance(LocationOf(p), LocationOf(drone)))
                    .FirstOrDefault(p => CanDroneMakeTrip(drone, p));
            }
        }
        #endregion
    }
}