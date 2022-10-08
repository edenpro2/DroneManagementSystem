﻿using BL.BO;
using DalFacade.DO;
using System;
using System.Linq;
using static BL.BO.BlPredicates;
using static BL.BO.GIS;
using static DalFacade.DO.DroneStatuses;

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
                                if (drone.battery < 100)
                                {
                                    drone.status = Maintenance;
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

                    else // On its way to a station:
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
                        var parcel = GetParcels(p => p.active).First(p => p.droneId == drone.id);
                        if (!CanDroneMakeTrip(drone, parcel))
                            progress = $"Parcel (id {parcel.id}) assigned to drone, but not enough battery to make trip";

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
                            case BlNotEnoughBatteryException:
                                drone.status = Maintenance;
                                progress =
                                    "Drone was in delivery, but doesn't have enough battery (now in maintenance)";
                                break;
                            case BlNoMatchingParcels:
                                drone.status = Maintenance;
                                break;
                        }
                        break;
                    }
            }

            UpdateDrone(drone);
            return new Tuple<Drone, string>(drone, progress);
        }
    }
}
