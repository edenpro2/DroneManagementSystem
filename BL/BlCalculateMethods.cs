using DO;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using static BO.DistanceFinder;

namespace BL
{
    public partial class Bl
    {
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

            if (drone.location == null)
            {
                return rate;
            }

            switch (drone.status)
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
                        var weight = GetParcels(p => p.active).FirstOrDefault(p => p.droneId == drone.id).weight;

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
            return drone.battery - consumption > 0;
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