using System;
using System.Collections.Generic;
using API.Controllers;

namespace API.Services
{
    public interface ITripMerge
    {
        Boolean TripCanBeMerged(int tripId);
        List<TripMergeDTO> GetTripsForMerging(int tripId);
        List<object> GetTripDates(int id1, int id2);
        List<int> GetTripEmployeesIds(int id1, int id2);
        int MergeTrips(MergeTripsData mergeTripsData);
    }
}