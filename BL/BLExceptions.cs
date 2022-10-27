using DalFacade.DO;
using System;

namespace BL
{
    public class BlNotFoundException : Exception
    {
        public BlNotFoundException(object o) : base($"{o.GetType()} not found") { }
    }

    public class BlAlreadyExistsException : Exception
    {
        public BlAlreadyExistsException(object o) : base($"{o} already exists") { }
    }

    public class BlDroneNotFreeException : Exception
    {
        public BlDroneNotFreeException() : base("Drone isn't free") { }
    }

    public class BlDroneNotMaintainedException : Exception
    {
        public BlDroneNotMaintainedException() : base("Drone is not currently in maintenance") { }
    }

    public class EmptyParameterException : Exception
    {
        public EmptyParameterException(Type paramType) : base($"Item {paramType} passed is empty or null") { }
    }

    public abstract class BlNotEnoughBatteryException : Exception
    {
        protected BlNotEnoughBatteryException() : base("Not enough battery to make trip") { }
    }

    public class BlNoOpenSlotsException : Exception
    {
        public BlNoOpenSlotsException(Station s) : base($"Station with ID:{s.Id} has no open slots") { }
    }

    public class BlNoMatchingParcels : Exception
    {
        public BlNoMatchingParcels() : base("No parcel is awaiting collection") { }
    }

    public class BlNotBeingDeliveredException : Exception
    {
        public BlNotBeingDeliveredException() : base("No parcel is being delivered by drone") { }
    }

    public class BlAlreadyCollected : Exception
    {
        public BlAlreadyCollected(Parcel p) : base($"Parcel (ID#{p.Id}) already collected") { }
    }
}