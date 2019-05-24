using System;
using System.Collections.Generic;
using System.Linq;
using API.Repositories;

namespace API.Services
{
    public class TripMerge : ITripMerge
    {
        private readonly IRepository<Trip> repository;

        public TripMerge(IRepository<Trip> tripRepo)
        {
            repository = tripRepo;
        }
        
        public bool TripCanBeMerged(int tripId)
        {
            Trip currentTrip = repository.Get(tripId);
            
            //if the trip starts earlier than tomorrow
            if (currentTrip.DepartureDate.Date <= DateTime.Today)
                return false;

            List<Trip> mergeTrips = GetMergeTrips(currentTrip);
            if (mergeTrips.Count != 0)
                return true;
            
            return false;
        }

        public List<Trip> GetMergeTrips(Trip currentTrip)
        {
            //get all trips where departure ar arrival dates -1 <= x <= 1
            List<Trip> mergableTrips = repository.GetAll(
                x => x.DepartureDate.Date <= currentTrip.DepartureDate.Date.AddDays(1) &&
                     x.DepartureDate.Date >= currentTrip.DepartureDate.Date.AddDays(-1) &&
                     x.ReturnDate.Date <= currentTrip.ReturnDate.Date.AddDays(1) &&
                     x.ReturnDate.Date >= currentTrip.ReturnDate.Date.AddDays(-1) &&
                     x.TripID != currentTrip.TripID).ToList();
            
            
            //if the current trip start tomorrow, do not let merge with the trips that start today
            if (currentTrip.DepartureDate.Date == DateTime.Today.AddDays(1))
                mergableTrips = mergableTrips.Where(x => x.DepartureDate.Date != DateTime.Today).ToList();

            if (currentTrip.ReturnDate.Date == DateTime.Today.AddDays(1))
                mergableTrips = mergableTrips.Where(x => x.ReturnDate.Date != DateTime.Today).ToList();

            if (currentTrip.IsPlaneNeeded && currentTrip.PlaneTickets.Count != 0)
                mergableTrips = mergableTrips.Where(x => x.IsPlaneNeeded && (x.PlaneTickets.Count == 0 ||
                                                                             (x.DepartureDate.Date.Equals(currentTrip
                                                                                  .DepartureDate.Date) &&
                                                                              x.ReturnDate.Date.Equals(
                                                                                  currentTrip.ReturnDate.Date)))).ToList();

            return mergableTrips;
        }
    }
}