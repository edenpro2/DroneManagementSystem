﻿#nullable enable
using DalFacade;
using DalFacade.DO;
using DALXML;
using System;
using System.Collections.Generic;
using System.Linq;
using static BL.BO.LocationFinder;

namespace BL
{
    public partial class Bl : BlApi
    {
        private static BlApi? _instance;
        public static BlApi Instance => _instance ??= new Bl();

        #region Constants
        private const int MinUnassignedBattery = 70;
        #endregion

        #region Attributes
        private static readonly DalApi DalApi = DalFactory.GetDal(nameof(DalXml));
        private List<Drone> _drones = DalApi.GetDrones().ToList();
        private readonly double[] _droneConsumptionRates;
        private readonly double _droneChargeRate;
        private static int _droneId;
        #endregion

        private Bl()
        {
            #region Pre-initialization
            var rates = DalApi.RequestPowerConsumption().ToList();
            _droneConsumptionRates = rates.GetRange(0, 4).ToArray();
            _droneChargeRate = rates.ElementAt(4);

            _droneId = _drones.Count;
            var parcels = DalApi.GetParcels().ToList();
            var customers = DalApi.GetCustomers().ToList();
            var rand = new Random();
            #endregion

            // All parcels have different stages...but no drone assigned to them yet
            foreach (var p in parcels)
            {
                var drone = Randomize.GetFreeRandomDrone(rand, _drones);

                // if drone isn't active, skip it (or no free drone found)
                if (drone == null || drone.Active == false)
                    break;

                var parcel = p;
                parcel.DroneId = drone.Id;

                switch (rand.Next(3))
                {
                    // Delivered
                    case 0:
                        // Set date sequence
                        parcel.Scheduled = parcel.Requested.AddHours(rand.Next(72));
                        parcel.Collected = parcel.Scheduled.AddDays(rand.Next(4));
                        parcel.Delivered = parcel.Collected.AddDays(rand.Next(2));
                        parcel.Active = false;
                        // Set location to target 
                        drone.Location = LocationOf(customers.First(c => c.Id == parcel.TargetId));
                        drone.Status = DroneStatuses.Free;
                        drone.Battery = rand.Next((int)MinForTripToStation(drone), 101);
                        break;

                    // Collected
                    case 1:
                        // Set date sequence
                        parcel.Scheduled = parcel.Requested.AddHours(rand.Next(72));
                        parcel.Collected = parcel.Scheduled.AddDays(rand.Next(4));
                        // Set location to sender 
                        drone.Location = LocationOf(customers.First(c => c.Id == parcel.SenderId));
                        drone.Status = DroneStatuses.Delivery;
                        drone.Battery = rand.Next((int)MinForDelivery(drone, parcel), 101);
                        break;

                    // Scheduled
                    case 2:
                        // Set date sequence
                        parcel.Scheduled = parcel.Requested.AddHours(rand.Next(72));
                        // Set location to closest station to sender 
                        drone.Location = LocationOf(this.GetClosestStation(customers.First(c => c.Id == parcel.SenderId)));
                        drone.Status = DroneStatuses.Delivery;
                        drone.Battery = rand.Next((int)MinForCollection(drone, parcel), 101);
                        break;
                }

                UpdateDrone(drone);
                UpdateParcel(parcel);
            }

            // Only drones that aren't in delivery will be processed 
            foreach (var d in _drones.Where(d => d.Status is null or not DroneStatuses.Delivery))
            {
                // randomize status
                d.Status = Randomize.GetRandomStatus(rand, 2);

                // Check drone status
                switch (d.Status)
                {
                    // Since drone is in maintenance, it needs to be charged at a random station with open slots
                    case DroneStatuses.Maintenance:
                        var freeStation = GetStations().Where(s => s.OpenSlots > 0).OrderBy(_ => rand.Next()).First();
                        freeStation.OpenSlots--;
                        freeStation.Ports.Add(new DroneCharge(d.Id, freeStation.Id));
                        UpdateStation(freeStation);
                        d.Location = LocationOf(freeStation);
                        d.Battery = rand.Next(MinUnassignedBattery, 101);
                        break;

                    // Drone is currently free, so its battery should be somewhat high and location at some random customer
                    case DroneStatuses.Free:
                        d.Battery = rand.Next(MinUnassignedBattery, 101);
                        d.Location = LocationOf(customers[rand.Next(customers.Count)]);
                        break;
                }

                UpdateDrone(d);
            }
        }
    }
}