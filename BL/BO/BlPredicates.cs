using DO;
using System;

namespace BO
{
    public static class BlPredicates
    {
        // Requested --> Scheduled --> Collected --> Delivered
        public static readonly Predicate<Parcel> NotAssignedToDrone = p => p.scheduled == default;  // has been requested
        public static readonly Predicate<Parcel> WaitingForDrone = p => p.collected == default;     // has been scheduled
        public static readonly Predicate<Parcel> InTransit = p => p.delivered == default;           // has been collected
        public static readonly Predicate<Parcel> AlreadyDelivered = p => p.delivered != default;    // has been delivered
    }
}