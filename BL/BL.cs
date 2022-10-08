using BLAPI;
using DalFacade;
using DalFacade.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using static BL.BO.LocationFinder;

namespace BL
{
    public partial class Bl : BlApi
    {
        private static BlApi _instance;
        public static BlApi Instance => _instance ??= new Bl();

        #region Constants
        private const int NumOfRates = 4;
        private const int MinBattery = 70;
        #endregion

        #region Attributes
        private static readonly DalApi DalApi = DalFactory.GetDal("DalXml");
        private List<Drone> _drones = DalApi.GetDrones().ToList();
        private readonly double[] _droneConsumptionRates = new double[NumOfRates];
        private readonly double _droneChargeRate;
        private static int _droneId;
        #endregion

        private Bl()
        {
            #region Pre-initialization
            int i;
            var rates = DalApi.RequestPowerConsumption();

            for (i = 0; i < NumOfRates; i++) 
                _droneConsumptionRates[i] = rates[i];
            _droneChargeRate = rates[i];

            var parcels = DalApi.GetParcels().ToList();
            var customers = DalApi.GetCustomers().ToList();
            _droneId = _drones.Count;
            #endregion

            var rand = new Random();
            // All parcels have different stages...but no drone assigned to them yet
            foreach (var p in parcels)
            {
                var drone = GetFreeRandomDrone(rand, p);

                // if drone isn't active, skip it
                if (drone.active == false)
                    break;

                var parcel = p;
                parcel.droneId = drone.id;

                Customer customer;

                switch (rand.Next(3))
                {
                    // Delivered
                    case 0:
                        parcel.scheduled = parcel.requested.AddHours(rand.Next(72));
                        parcel.collected = parcel.scheduled.AddDays(rand.Next(4));
                        parcel.delivered = parcel.collected.AddDays(rand.Next(2));
                        parcel.active = false;
                        drone.status = DroneStatuses.Free;
                        customer = customers.First(c => c.id == parcel.targetId);
                        drone.location = Location(customer);
                        drone.battery = rand.Next(MinForTripToStation(drone), 101);
                        break;

                    // Collected
                    case 1:
                        parcel.scheduled = parcel.requested.AddHours(rand.Next(72));
                        parcel.collected = parcel.scheduled.AddDays(rand.Next(4));
                        customer = customers.First(c => c.id == parcel.senderId);
                        drone.status = DroneStatuses.Delivery;
                        drone.location = Location(customer);
                        drone.battery = rand.Next(MinForDelivery(drone, parcel), 101);
                        break;

                    // Scheduled
                    case 2:
                        parcel.scheduled = parcel.requested.AddHours(rand.Next(72));
                        var station = this.GetClosestStation(customers.First(c => c.id == parcel.senderId));
                        drone.status = DroneStatuses.Delivery;
                        drone.location = Location(station);
                        drone.battery = rand.Next(MinForCollection(drone, parcel), 101);
                        break;

                }

                UpdateDrone(drone);
                UpdateParcel(parcel);
            }

            // Only drones that aren't in delivery will be processed 
            foreach (var d in _drones.Where(d => d.status != DroneStatuses.Delivery))
            {
                var drone = d;

                // randomize status
                drone.status = GetRandomStatus(rand, 2);

                // Check drone status
                switch (drone.status)
                {
                    // Since drone is in maintenance, it needs to be charged at a random station with open slots
                    case DroneStatuses.Maintenance:
                        var freeStation = GetStations().Where(s => s.openSlots > 0).OrderBy(_ => rand.Next()).First();
                        freeStation.openSlots--;
                        freeStation.ports.Add(new DroneCharge(drone.id, freeStation.id));
                        UpdateStation(freeStation);
                        drone.location = Location(freeStation);
                        drone.battery = rand.Next(MinBattery, 101);
                        break;

                    // Drone is currently free, so its battery should be somewhat high and location at some random customer
                    case DroneStatuses.Free:
                        drone.battery = rand.Next(MinBattery, 101);
                        drone.location = Location(customers[rand.Next(customers.Count)]);
                        break;
                }

                UpdateDrone(drone);
            }

        }

        private Drone GetFreeRandomDrone(Random rand, Parcel parcel)
        {
            return _drones.
                Where(d => d.status is DroneStatuses.Free or null).
                OrderBy(_ => rand.Next()).
                FirstOrDefault(d => CanDroneMakeTrip(d, parcel));
        }

        private int MinForTripToStation(Drone drone)
        {
            var station = this.ClosestAvailableStation(drone);
            return (int)CalcPowerConsumption(drone, station);
        }

        private int MinForDelivery(Drone drone, Parcel parcel)
        {
            var target = GetCustomers(c => c.active).First(c => c.id == parcel.targetId);
            var total = (int)CalcPowerConsumption(drone, target);
            drone.location = Location(target);
            total += MinForTripToStation(drone);
            return total;
        }

        private int MinForCollection(Drone drone, Parcel parcel)
        {
            var sender = GetCustomers(c => c.active).First(c => c.id == parcel.senderId);
            var total = (int)CalcPowerConsumption(drone, sender);
            drone.location = Location(sender);
            total += MinForDelivery(drone, parcel);
            return total;
        }

        private static DroneStatuses GetRandomStatus(Random rand, short bound)
        {
            return (DroneStatuses)rand.Next(bound);
        }
    }

}