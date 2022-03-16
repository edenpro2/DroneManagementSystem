using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static BO.BlPredicates;
using static BO.DistanceFinder;
using static BO.LocationFinder;
using static DO.DroneStatuses;

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
                if (drones[i].id == drone.id)
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
            dalApi.UpdateStation(station);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(Customer customer)
        {
            dalApi.UpdateCustomer(customer);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcel(Parcel parcel)
        {
            dalApi.UpdateParcel(parcel);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateUser(User user)
        {
            dalApi.UpdateUser(user);
        }

        #endregion

        #region Update Entire Lists
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneList(List<Drone> drones)
        {
            dalApi.UpdateDroneList(_drones = drones);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStationList(List<Station> stations)
        {
            dalApi.UpdateStationList(stations);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomerList(List<Customer> customers)
        {
            dalApi.UpdateCustomerList(customers);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcelList(List<Parcel> parcels)
        {
            dalApi.UpdateParcelList(parcels);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateUserList(List<User> users)
        {
            dalApi.UpdateUserList(users);
        }
        #endregion

        #region Drone updates

        /// <summary>
        /// Drone is added to ports in station (location = station)
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        /// <exception cref="BLDroneNotMaintainedException"></exception>
        /// <exception cref="BLNoOpenSlotsException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone ReceiveDroneForCharging(Drone drone)
        {
            if (drone.status is not Maintenance)
            {
                throw new BLDroneNotMaintainedException();
            }

            var closestStation = this.ClosestAvailableStation(drone);

            if (closestStation.Equals(default))
            {
                throw new BLNoOpenSlotsException();
            }

            var closestLocation = Location(closestStation);

            lock (dalApi)
            {
                closestStation.openSlots--;
                closestStation.ports.Add(new DroneCharge(closestStation.id, drone.id));
                UpdateStation(closestStation);

                drone.location = closestLocation;
                UpdateDrone(drone);

                return drone;
            }
        }

        /// <summary>
        /// Drone is sent to charge at closest station (location unchanged)
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        /// <exception cref="BLDroneNotFreeException"></exception>
        /// <exception cref="BLNoOpenSlotsException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone SendDroneToCharge(Drone drone)
        {
            if (drone.status is not Free)
            {
                throw new BLDroneNotFreeException();
            }

            var closestStation = this.ClosestAvailableStation(drone);

            if (closestStation.Equals(default))
            {
                throw new BLNoOpenSlotsException();
            }

            lock (dalApi)
            {
                closestStation.openSlots--;
                closestStation.ports.Add(new DroneCharge(closestStation.id, drone.id));
                UpdateStation(closestStation);

                drone.status = Maintenance;
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

            if (drone.battery + total > 100)
            {
                drone.battery = 100;
            }
            else
            {
                drone.battery += total;
            }

            UpdateDrone(drone);
            return drone;
        }

        /// <summary>
        /// Charges drone for x hours and releases it (location unchanged). This is for non-threaded simulation
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        /// <exception cref="BLDroneNotMaintainedException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone DroneReleaseAndCharge(Drone drone, int hours)
        {
            if (drone.status is not Maintenance)
            {
                throw new BLDroneNotMaintainedException();
            }

            var droneLocation = Location(drone);

            lock (dalApi)
            {
                var batteryAfterCharge = _droneChargeRate * hours;

                if (drone.battery + batteryAfterCharge > 100)
                {
                    drone.battery = 100;
                }
                else
                {
                    drone.battery += batteryAfterCharge;
                }

                drone.status = Free;

                UpdateDrone(drone);

                // Get station with same location as drone
                var station = GetStations().First(s => Location(s).Equals(droneLocation));
                station.openSlots++;
                station.ports.Remove(station.ports.Find(p => p.droneId == drone.id));
                UpdateStation(station);

                return drone;
            }
        }

        /// <summary>
        /// Release drone from charging and delete port (location unchanged)
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        /// <exception cref="BLDroneNotMaintainedException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone DroneRelease(Drone drone)
        {
            if (drone.status is not Maintenance)
            {
                throw new BLDroneNotMaintainedException();
            }

            lock (dalApi)
            {
                drone.status = Free;
                UpdateDrone(drone);

                // Delete droneCharge
                var station = this.GetClosestStation(drone);
                if (station.ports.Exists(p => p.droneId == drone.id))
                {
                    station.openSlots++;
                    station.ports.Remove(station.ports.First(p => p.droneId == drone.id));
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
        /// <exception cref="BLDroneNotFreeException"></exception>
        /// <exception cref="BLNoMatchingParcels"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone AssignDroneToParcel(Drone drone)
        {
            // can't assign if not free
            if (drone.status is not Free)
            {
                throw new BLDroneNotFreeException();
            }

            // look for the highest priority parcel and closest available drone 
            var parcel = BestMatchingParcel(drone);

            // if parcel not found will be automatically default
            if (parcel.Equals(default) || parcel.active is false)
            {
                throw new BLNoMatchingParcels();
            }

            lock (dalApi)
            {
                // update both parcel and drone
                parcel.droneId = drone.id;
                parcel.scheduled = DateTime.Now;
                UpdateParcel(parcel);
                drone.status = Delivery;
                UpdateDrone(drone);

                return drone;
            }
        }

        /// <summary>
        /// Collects the parcel and (location = parcel)
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        /// <exception cref="BLNotFoundException"></exception>
        /// <exception cref="BLAlreadyCollected"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone CollectParcelByDrone(Drone drone)
        {
            var parcel = GetParcels(p => p.active).FirstOrDefault(p => p.droneId == drone.id);

            if (parcel.requested == default)
            {
                throw new BLNotFoundException();
            }

            if (!WaitingForDrone(parcel))
            {
                throw new BLAlreadyCollected();
            }

            lock (dalApi)
            {
                var parcelLocation = Location(parcel);

                drone.location = parcelLocation;
                UpdateDrone(drone);

                parcel.collected = DateTime.Now;
                UpdateParcel(parcel);

                return drone;
            }
        }

        /// <summary>
        /// Delivers parcel to addressee (location = target)
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        /// <exception cref="BLNotBeingDeliveredException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone DeliverByDrone(Drone drone)
        {
            var parcel = GetParcels(p => p.active).FirstOrDefault(p => p.droneId == drone.id);

            if (parcel.delivered != default || parcel.active == false)
            {
                throw new BLNotBeingDeliveredException();
            }

            lock (dalApi)
            {
                drone.status = Free;
                drone.location = Location(SearchForCustomer(c => c.id == parcel.targetId));
                UpdateDrone(drone);

                parcel.delivered = DateTime.Now;
                parcel.active = false;
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
            var change = 0.0;
            var id = drone.id;
            switch (drone.status)
            {
                case Delivery:
                    var parcel = GetParcels(p => p.active).First(p => p.droneId == id);

                    // drone hasn't collected parcel yet
                    if (WaitingForDrone(parcel) || NotAssignedToDrone(parcel))
                    {
                        change = -ConsumptionWhenFree();
                    }

                    // drone is delivering parcel
                    else
                    {
                        switch (parcel.weight)
                        {
                            case WeightCategories.Light:
                                change = -ConsumptionWhenLight();
                                break;
                            case WeightCategories.Medium:
                                change = -ConsumptionWhenMid();
                                break;
                            case WeightCategories.Heavy:
                                change = -ConsumptionWhenHeavy();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    break;
                case Maintenance:
                    // drone is at the station (negate change)
                    if (Location(drone).Equals(Location(this.GetClosestStation(drone))))
                    {
                        change = _droneChargeRate;

                        // if overflow detected, change is adjusted
                        if (drone.battery + change > 100)
                        {
                            change = 100 - drone.battery;
                        }
                    }
                    // drone is on it's way to station (hence it's free)
                    else
                    {
                        change = -ConsumptionWhenFree();
                    }

                    break;
                case Free:
                    // drone is at station (idle state)
                    if (Location(drone).Equals(Location(this.GetClosestStation(drone))))
                    {
                        change = 0;
                    }
                    // drone is on it's way to station (and free)
                    else
                    {
                        change = -ConsumptionWhenFree();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            drone.battery += change;
            return drone;
        }

        private Parcel BestMatchingParcel(Drone drone)
        {
            lock (dalApi)
            {
                return
                    GetParcels(p => p.active)
                    .Where(p => NotAssignedToDrone(p))
                    .Where(p => p.weight <= drone.maxWeight)
                    .OrderByDescending(p => p.priority)
                    .ThenByDescending(p => p.weight)
                    .ThenBy(p => Distance(Location(p), Location(drone)))
                    .FirstOrDefault(p => CanDroneMakeTrip(drone, p));
            }
        }



        #endregion
    }
}