using BL.BO;
using DalFacade.DO;
using System;
using System.Linq;
using static BL.BO.BlPredicates;
using static BL.BO.GeoInfoSystem;
using static DalFacade.DO.DroneStatuses;

namespace BL
{
    public sealed partial class Bl
    {
        public Tuple<Drone, string> DroneSimulator(Drone d)
        {
            var drone = d;
            var progress = "";

            switch (drone.Status)
            {
                case Free:
                    try // Drone status set to delivery if no exceptions
                    {
                        drone = AssignDroneToParcel(drone);
                    }
                    catch (Exception ex)
                    {
                        switch (ex)
                        {
                            case BlNoMatchingParcels:
                                // If non-full battery, drone status will be maintenance, so send it to closest station
                                if (drone.Battery < 100)
                                {
                                    drone.Status = Maintenance;
                                    progress = "Will be sent to nearest charging port";
                                }
                                else progress = "Currently idle";

                                break;
                        }
                    }
                    break;
                // ______________________________________________________________________________________________________
                case Maintenance:

                    var station = this.ClosestAvailableStation(drone);

                    // Drone is at a station:
                    if (LocationOf(drone).Equals(LocationOf(station)))
                    {
                        switch (drone.Battery)
                        {
                            case < 100:
                                drone = UpdateBattery(drone);
                                progress = "Charging battery";
                                break;

                            default:
                                drone = DroneRelease(drone);
                                progress = "Released from charging port";
                                break;
                        }
                    }

                    else // On its way to a station:
                    {
                        // Drone isn't at station yet
                        if (Distance(LocationOf(drone), LocationOf(station)) > this.Speed(drone))
                        {
                            drone = UpdateBattery(drone);
                            drone.Location = this.CalculateLocation(drone, LocationOf(station));
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
                                    case BlDroneNotFreeException:
                                        //TODO: Send drone rescue 
                                        progress = "Drone was near the charging port but isn't free";
                                        break;

                                    case BlNoOpenSlotsException:
                                        //TODO: No open slots solution
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
                        var parcel = GetParcels(p => p.Active).First(p => p.DroneId == drone.Id);
                        if (!CanDroneMakeTrip(drone, parcel))
                        {
                            progress = $"Parcel (id {parcel.Id}) assigned to drone, but not enough battery to make trip";
                            break;
                        }

                        if (WaitingForDrone(parcel))
                        {
                            if (Distance(LocationOf(drone), LocationOf(parcel)) > this.Speed(drone))
                            {
                                drone.Location = this.CalculateLocation(drone, LocationOf(parcel));
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
                            var customer = GetCustomers(c => c.Active).First(c => c.Id == parcel.TargetId);
                            if (Distance(LocationOf(drone), LocationOf(customer)) > this.Speed(drone))
                            {
                                drone.Location = this.CalculateLocation(drone, LocationOf(customer));
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
                        drone.Status = Maintenance;
                        progress = exception.Message;
                        break;
                    }
            }

            UpdateDrone(drone);

            return new Tuple<Drone, string>(drone, progress);
        }
    }
}
