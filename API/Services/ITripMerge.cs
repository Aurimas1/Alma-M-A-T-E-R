using System;

namespace API.Services
{
    public interface ITripMerge
    {
        Boolean TripCanBeMerged(int tripId);
    }
}