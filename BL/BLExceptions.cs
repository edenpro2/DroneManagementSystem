using System;

namespace BL
{
    //Item not found
    public class BLNotFoundException : Exception { }

    //Item already exists
    public class BLAlreadyExistsException : Exception { }

    //Drone isn't free
    public class BLDroneNotFreeException : Exception { }

    //Drone is not currently in maintenance
    public class BLDroneNotMaintainedException : Exception { }

    //Empty parameter passed
    public class EmptyParameterException : Exception { }

    //Drone doesn't have enough battery
    public class BLNotEnoughBatteryException : Exception { }

    //Station doesn't have an open charge slot
    public class BLNoOpenSlotsException : Exception { }

    //Parcel is not awaiting collection
    public class BLNoMatchingParcels : Exception { }

    //Parcel is not being delivered
    public class BLNotBeingDeliveredException : Exception { }

    //Parcel already collected
    public class BLAlreadyCollected : Exception { }
}