﻿using DalFacade.DO;
using System;

namespace BL.BO
{
    public static class BlPredicates
    {
        // Requested --> Scheduled --> Collected --> Delivered
        public static readonly Predicate<Parcel> NotAssignedToDrone = p => p.Scheduled == default;                            // has been requested
        public static readonly Predicate<Parcel> WaitingForCollection = p => p.Collected == default && p.Scheduled != default;     // has been scheduled
        public static readonly Predicate<Parcel> InTransit = p => p.Delivered == default && p.Collected != default;           // has been collected
    }
}