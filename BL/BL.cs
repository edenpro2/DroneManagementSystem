using DalFacade;
using DalFacade.DO;
using DALXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using static BL.BO.LocationFinder;

namespace BL
{
    public partial class Bl : BlApi
    {
        #region Singleton
        private static BlApi _instance;
        public static BlApi Instance => _instance ??= new Bl();
        #endregion

        #region Constants
        private const int MIN_UNASSIGNED_BATTERY = 70;
        private const int MIN_UNTIL_SCHEDULED = 40;
        private const int MAX_UNTIL_SCHEDULED = 60;
        private const int MIN_UNTIL_COLLECTED = 10;
        private const int MAX_UNTIL_COLLECTED = 20;
        private const int MIN_UNTIL_DELIVERED = 10;
        private const int MAX_UNTIL_DELIVERED = 18;
        private const int MAX_BATTERY = 100;
        #endregion

        #region Attributes
        private static readonly DalApi DalApi = DalFactory.GetDal(nameof(DalXml));
        private List<Drone> _drones = DalApi.GetDrones().ToList();
        private readonly double[] _droneConsumptionRates;
        private readonly double _droneChargeRate;
        private static int _droneId;
        private readonly Random _rand = new();
        #endregion


        private Bl()
        {
            var rates = DalApi.RequestPowerConsumption().ToList();
            _droneConsumptionRates = rates.GetRange(0, 4).ToArray();
            _droneChargeRate = rates.ElementAt(4);
            _droneId = _drones.Count;

            var parcels = CollectionsMarshal.AsSpan(DalApi.GetParcels().ToList());
            var customers = DalApi.GetCustomers().ToList();

            // Copy free drones into new list
            var freeDrones = new List<Drone>(_drones
                .Where(d => d.Active && d.Status is DroneStatuses.Free or null)
                .OrderBy(_ => _rand));

            /* All parcels will have different stages...but no drone has been assigned to them yet.
             * In each loop of freeDrones, we set all drone locations to a specific location.
             * Then we can check if a drone can fly from its given location to an end location */
            foreach (var p in parcels)
            {
                var parcel = p;
                var drone = new Drone();

                switch (_rand.Next(3))
                {
                    /* Delivered */
                    case 0:
                        // Check for free drones
                        drone = freeDrones.FirstOrDefault();
                        if (drone == default)
                            continue;

                        // Location of receiver
                        drone.Location = customers.First(c => c.Id == parcel.TargetId).Location;
                        if ((int)MinForTripToStation(drone) > MAX_BATTERY)
                            continue;

                        drone.Battery = _rand.Next((int)MinForTripToStation(drone), MAX_BATTERY);
                        drone.Status = DroneStatuses.Free;
                        // Set date sequence
                        parcel.Scheduled = parcel.Requested.AddHours(_rand.Next(MIN_UNTIL_SCHEDULED, MAX_UNTIL_SCHEDULED));
                        parcel.Collected = parcel.Scheduled.AddHours(_rand.Next(MIN_UNTIL_COLLECTED, MAX_UNTIL_COLLECTED));
                        parcel.Delivered = parcel.Collected.AddHours(_rand.Next(MIN_UNTIL_DELIVERED, MAX_UNTIL_DELIVERED));
                        parcel.Active = false;
                        break;

                    /* Collected */
                    case 1:
                        // Check for free drones
                        drone = freeDrones.FirstOrDefault();
                        if (drone == default)
                            continue;

                        // Location of sender
                        drone.Location = customers.First(c => c.Id == parcel.SenderId).Location;
                        if ((int)MinForDelivery(drone, parcel) > MAX_BATTERY)
                            continue;

                        drone.Battery = _rand.Next((int)MinForDelivery(drone, parcel), MAX_BATTERY);
                        drone.Status = DroneStatuses.Delivery;
                        // Set date sequence
                        parcel.Scheduled = parcel.Requested.AddHours(_rand.Next(MIN_UNTIL_SCHEDULED, MAX_UNTIL_SCHEDULED));
                        parcel.Collected = parcel.Scheduled.AddHours(_rand.Next(MIN_UNTIL_COLLECTED, MAX_UNTIL_COLLECTED));
                        // Set location to sender 
                        break;

                    /* Scheduled */
                    case 2:
                        // Check for free drones
                        drone = freeDrones.FirstOrDefault();
                        if (drone == default)
                            continue;

                        // Closest station to sender location
                        drone.Location = this.GetClosestStation(customers.First(c => c.Id == parcel.SenderId)).Location;
                        if ((int)MinForDelivery(drone, parcel) > MAX_BATTERY)
                            continue;

                        // Set location to closest station to sender 
                        drone.Battery = _rand.Next((int)MinForCollection(drone, parcel), MAX_BATTERY);
                        drone.Status = DroneStatuses.Delivery;
                        // Set date sequence
                        parcel.Scheduled = parcel.Requested.AddHours(_rand.Next(MIN_UNTIL_SCHEDULED, MAX_UNTIL_SCHEDULED));
                        break;
                }

                parcel.DroneId = drone.Id;
                freeDrones = freeDrones.Where(d => d.Id != drone.Id).ToList();
                UpdateDrone(drone);
                UpdateParcel(parcel);
            }

            // Only drones that aren't in delivery will be processed 
            foreach (var d in CollectionsMarshal.AsSpan(_drones.Where(d => d.Status != DroneStatuses.Delivery).ToList()))
            {
                switch (_rand.Next(2))
                {
                    // Drone is currently free, so its battery should be somewhat high and location at some random customer
                    case 0:
                        // Update drone
                        d.Status = DroneStatuses.Free;
                        d.Battery = _rand.Next(MIN_UNASSIGNED_BATTERY, 101);
                        d.Location = LocationOf(customers[_rand.Next(customers.Count)]);
                        UpdateDrone(d);
                        break;

                    // Since drone is in maintenance, it needs to be charged at a random station with open slots
                    case 1:
                        // Update station
                        var freeStation = GetStations().Where(s => s.OpenSlots > 0).OrderBy(_ => _rand.Next()).First(); //*
                        freeStation.OpenSlots--;
                        freeStation.Ports.Add(new DroneCharge(d.Id, freeStation.Id));
                        UpdateStation(freeStation);
                        // Update drone
                        d.Status = DroneStatuses.Maintenance;
                        d.Location = LocationOf(freeStation);
                        d.Battery = _rand.Next(MIN_UNASSIGNED_BATTERY, 101);
                        UpdateDrone(d);
                        break;
                }
            }
        }
    }
}