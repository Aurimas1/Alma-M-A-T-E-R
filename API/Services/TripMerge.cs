using System;
using System.Collections.Generic;
using System.Linq;
using API.Repositories;

namespace API.Services
{
    public class TripMerge : ITripMerge
    {
        private readonly IRepository<Trip> tripRepository;
        private readonly IRepository<Employee> employeeRepository;

        public TripMerge(IRepository<Trip> tripRepo, IRepository<Employee> empRepo)
        {
            tripRepository = tripRepo;
            employeeRepository = empRepo;
        }
        
        public bool TripCanBeMerged(int tripId)
        {
            Trip currentTrip = tripRepository.Get(tripId);
            
            //if the trip starts earlier than tomorrow
            if (currentTrip.DepartureDate.Date <= DateTime.Today)
                return false;

            List<Trip> mergeTrips = GetMergeTrips(currentTrip);
            if (mergeTrips.Count != 0)
                return true;
            
            return false;
        }

        public List<TripMergeDTO> GetTripsForMerging(int tripId)
        {
            Trip currentTrip = tripRepository.Get(tripId);
            List<Trip> tripsForMerging = GetMergeTrips(currentTrip);
            List<TripMergeDTO> tripsForMergingDTO = new List<TripMergeDTO>();
            tripsForMerging.ForEach(x =>
            {
                string organiser = employeeRepository.Get(x.OrganizerID.GetValueOrDefault()).Name;
                tripsForMergingDTO.Add(new TripMergeDTO (currentTrip, organiser));
            });
            return tripsForMergingDTO;
        }
        

        private List<Trip> GetMergeTrips(Trip currentTrip)
        {
            //get all trips where departure ar arrival dates -1 <= x <= 1
            List<Trip> mergableTrips = tripRepository.GetAll(
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
    
    public class TripMergeDTO
    {
        public DateTime DepartureDate;
        public DateTime ReturnDate;
        public string organiser;
        public int BoughtPlaneTicketsCount;
        public int AccomodationCount;
        public int CarRentalCount;
        public int EmployeesCount;
        public int ConfirmedEmployeesCount;
        public int GasCompensationCount;
        public string DepartureOffice;
        public string ArrivalOffice;
        public Boolean PlaneTicketsNeeded;

            
        public TripMergeDTO(Trip trip, string organiser)
        {
            DepartureDate = trip.DepartureDate;
            ReturnDate = trip.ReturnDate;
            this.organiser = organiser;
            EmployeesCount = trip.EmployeesToTrip.Count;
            ConfirmedEmployeesCount = trip.EmployeesToTrip.Count(x => x.Status == "APPROVED");
            BoughtPlaneTicketsCount = trip.PlaneTickets.Count;
            PlaneTicketsNeeded = trip.IsPlaneNeeded;
            AccomodationCount = trip.Reservations.Count;
            GasCompensationCount = trip.GasCompensations.Count;
            CarRentalCount = trip.CarRentals.Count;
            DepartureOffice = trip.DepartureOffice.City + ", " + trip.DepartureOffice.Country;
            ArrivalOffice = trip.ArrivalOffice.City + ", " + trip.ArrivalOffice.Country;

        }
    }
    
}