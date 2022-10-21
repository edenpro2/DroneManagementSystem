using DalFacade.DO;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using BL.BO;
using static BL.BO.GIS;

namespace BL
{
    public partial class Bl
    {
        private int MinForTripToStation(Drone drone)
        {
            var station = this.ClosestAvailableStation(drone);
            return (int)CalcPowerConsumption(drone, station);
        }

        private int MinForDelivery(Drone drone, Parcel parcel)
        {
            var target = GetCustomers(c => c.Active).First(c => c.Id == parcel.TargetId);
            var total = (int)CalcPowerConsumption(drone, target);
            drone.Location = Location(target);
            total += MinForTripToStation(drone);
            return total;
        }

        private int MinForCollection(Drone drone, Parcel parcel)
        {
            var sender = GetCustomers(c => c.Active).First(c => c.Id == parcel.SenderId);
            var total = (int)CalcPowerConsumption(drone, sender);
            drone.Location = Location(sender);
            total += MinForDelivery(drone, parcel);
            return total;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double CalcPowerConsumption(Drone drone, object o)
        {
            Location targetLocation;
            switch (o)
            {
                case Station station:
                    targetLocation = Location(station);
                    break;
                case Customer customer:
                    targetLocation = Location(customer);
                    break;
                case Parcel parcel:
                    targetLocation = Location(parcel);
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

            var distance = Distance(Location(drone), targetLocation);
            return rate * distance;
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
    }
}