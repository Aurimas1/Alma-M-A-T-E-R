using System;
using System.Collections.Generic;

namespace API.Services
{
    public interface ITripMerge
    {
        Boolean TripCanBeMerged(int tripId);
        List<TripMergeDTO> GetTripsForMerging(int tripId);
    }
}