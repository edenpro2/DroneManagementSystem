using System;

namespace BL
{
    //Item not found
    public class BlNotFoundException : Exception { }

    //Item already exists
    public class BlAlreadyExistsException : Exception { }

    //Drone isn't free
    public class BlDroneNotFreeException : Exception { }

    //Drone is not currently in maintenance
    public class BlDroneNotMaintainedException : Exception { }

    //Empty parameter passed
    public class EmptyParameterException : Exception { }

    //Drone doesn't have enough battery
    public abstract class BlNotEnoughBatteryException : Exception { }

    //Station doesn't have an open charge slot
    public class BlNoOpenSlotsException : Exception { }

    //Parcel is not awaiting collection
    public class BlNoMatchingParcels : Exception { }

    //Parcel is not being delivered
    public class BlNotBeingDeliveredException : Exception { }

    //Parcel already collected
    public class BlAlreadyCollected : Exception { }
}