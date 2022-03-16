using BO;
using DO;
using System;
using System.Linq;
using static BO.BlPredicates;
using static BO.DistanceFinder;
using static DO.DroneStatuses;

namespace BL
{
    public partial class Bl
    {
        public Tuple<Drone, string> DroneSimulator(Drone d)
        {
            var drone = d;
            var progress = "";
            switch (drone.status)
            {
                // ______________________________________________________________________________________________________
                case Free:

                    try
                    {
                        // Drone status set to delivery if no exceptions
                        drone = AssignDroneToParcel(drone);
                    }
                    catch (Exception ex)
                    {
                        switch (ex)
                        {
                            case BLNoMatchingParcels:
                                // If non-full battery, drone status will be maintenance, so send it to closest station
                                if (drone.battery < 100)
                                {
                                    drone.status = Maintenance;
                                    progress = "Will be sent to nearest charging port";
                                }
                                else
                                {
                                    progress = "Currently idle";
                                }

                                break;
                        }
                    }
                    break;
                // ______________________________________________________________________________________________________
                case Maintenance:

                    // search for closest station for charging
                    var station = this.ClosestAvailableStation(drone);

                    // drone is at station
                    if (Location(drone).Equals(Location(station)))
                    {
                        switch (drone.battery)
                        {
                            case < 100:
                                drone = UpdateBattery(drone);
                                progress = "Charging battery";
                                break;

                            default:
                                drone = DroneRelease(drone);
                                progress = "Released from charging";
                                break;
                        }
                    }
                    // drone is on its way to the station 
                    else
                    {
                        // Drone isn't at station yet
                        if (Distance(Location(drone), Location(station)) > this.Speed(drone))
                        {
                            drone = UpdateBattery(drone);
                            drone.location = this.CalculateLocation(drone, Location(station));
                            progress = "On its way to nearest charging port";
                        }
                        // Drone is right next to station
                        else
                        {
                            try
                            {
                                drone = ReceiveDroneForCharging(drone);
                                progress = "Arrived at charging port";
                            }
                            catch (Exception chargeException)
                            {
                                switch (chargeException)
                                {
                                    case BLDroneNotFreeException:
                                        //TODO: Send drone rescue 
                                        progress = "Drone was near the charging port but isn't free";
                                        break;

                                    case BLNoOpenSlotsException:
                                        //TODO: No open slots
                                        progress = "Station doesn't have any open slots";
                                        break;
                                }
                            }
                        }
                    }
                    break;
                // ______________________________________________________________________________________________________
                case Delivery:

                    try
                    {
                        var parcel = GetParcels(p => p.active).First(p => p.droneId == drone.id);
                        if (!CanDroneMakeTrip(drone, parcel))
                        {
                            progress = $"Parcel (id {parcel.id}) assigned to drone, but not enough battery to make trip";
                        }

                        if (WaitingForDrone(parcel))
                        {
                            if (Distance(Location(drone), Location(parcel)) > this.Speed(drone))
                            {
                                drone.location = this.CalculateLocation(drone, Location(parcel));
                                drone = UpdateBattery(drone);
                                progress = "Drone on its way to collect parcel";
                            }
                            else
                            {
                                drone = CollectParcelByDrone(drone);
                                drone = UpdateBattery(drone);
                                progress = "Drone just collected the parcel";
                            }
                        }
                        else if (InTransit(parcel))
                        {
                            var customer = GetCustomers(c => c.active).First(c => c.id == parcel.targetId);
                            if (Distance(Location(drone), Location(customer)) > this.Speed(drone))
                            {
                                drone.location = this.CalculateLocation(drone, Location(customer));
                                drone = UpdateBattery(drone);
                                progress = "Delivering parcel to addressee";
                            }
                            else
                            {
                                drone = DeliverByDrone(drone);
                                progress = "Drone arrived at destination and delivered the parcel";
                            }
                        }

                        break;
                    }
                    catch (Exception exception)
                    {
                        switch (exception)
                        {
                            case BLNotEnoughBatteryException:
                                drone.status = Maintenance;
                                progress =
                                    "Drone was in delivery, but doesn't have enough battery (now in maintenance)";
                                break;
                            case BLNoMatchingParcels:
                                drone.status = Maintenance;
                                break;
                        }
                        break;
                    }
                    // ______________________________________________________________________________________________________

            }


            UpdateDrone(drone);

            return new Tuple<Drone, string>(drone, progress);
        }
    }
}
