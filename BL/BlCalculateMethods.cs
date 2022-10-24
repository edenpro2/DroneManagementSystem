#nullable enable
using BL.BO;
using DalFacade.DO;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using static BL.BO.BlPredicates;
using static BL.BO.GeoInfoSystem;

namespace BL
{
    public partial class Bl
    {
        // Minimum for collection + min for delivery + min for trip to closest station after delivery
        private double MinForCollection(Drone drone, Parcel parcel)
        {
            var min = CalcPowerConsumption(drone, parcel);
            min += MinForDelivery(drone, parcel);
            return min;
        }

        // Minimum for delivery + min for trip to closest station after delivery
        private double MinForDelivery(Drone drone, Parcel parcel)
        {
            var min = CalcPowerConsumption(drone, parcel);
            min += MinForTripToStation(drone);
            return min;
        }

        private double MinForTripToStation(Drone drone)
        {
            var station = this.ClosestAvailableStation(drone);
            return CalcPowerConsumption(drone, station);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double CalcPowerConsumption(Drone drone, object o)
        {
            Location targetLocation;
            switch (o)
            {
                case Station station:
                    targetLocation = LocationOf(station);
                    break;
                case Customer customer:
                    targetLocation = LocationOf(customer);
                    break;
                case Parcel parcel:
                    targetLocation = LocationOf(parcel);
                    break;
                default:
                    return 0.0;
            }

            var rate = 0.0;

            if (drone.Location == null)
            {
                return rate;
            }

            switch (drone.Status)
            {
                case DroneStatuses.Free:
                    rate = ConsumptionWhenFree();
                    break;
                case DroneStatuses.Maintenance:
                    rate = ConsumptionWhenFree();
                    break;
                default:
                    try
                    {
                        var weight = GetParcels(p => p.Active).First(p => p.DroneId == drone.Id).Weight;

                        switch (weight)
                        {
                            case WeightCategories.Light:
                                rate = ConsumptionWhenLight();
                                break;
                            case WeightCategories.Medium:
                                rate = ConsumptionWhenMid();
                                break;
                            case WeightCategories.Heavy:
                                rate = ConsumptionWhenHeavy();
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        rate = ConsumptionWhenLight();
                    }
                    break;
            }

            return rate * Distance(LocationOf(drone), targetLocation);
        }

        // Helper function
        private bool CanDroneMakeTrip(Drone drone, Parcel parcel)
        {
            var consumption = MinForCollection(drone, parcel);
            return drone.Battery - consumption > 0;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double ConsumptionWhenFree()
        {
            return _droneConsumptionRates[0];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double ConsumptionWhenLight()
        {
            return _droneConsumptionRates[1];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double ConsumptionWhenMid()
        {
            return _droneConsumptionRates[2];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double ConsumptionWhenHeavy()
        {
            return _droneConsumptionRates[3];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private Parcel? BestMatchingParcel(Drone drone)
        {
            return GetParcels(p => p.Active)
                .Where(p => NotAssignedToDrone(p))
                .Where(p => p.Weight <= drone.MaxWeight)
                .OrderByDescending(p => p.Priority)
                .ThenByDescending(p => p.Weight)
                .ThenBy(p => Distance(LocationOf(p), LocationOf(drone)))
                .FirstOrDefault(p => CanDroneMakeTrip(drone, p));
        }
    }
}