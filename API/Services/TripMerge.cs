using System;
using System.Collections.Generic;
using System.Linq;
using API.Controllers;
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
                tripsForMergingDTO.Add(new TripMergeDTO (x, organiser));
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
                     x.TripID != currentTrip.TripID && 
                     x.DepartureOfficeID == currentTrip.DepartureOfficeID &&
                     x.ArrivalOfficeID == currentTrip.ArrivalOfficeID).ToList();
            
            
            //if the current trip start tomorrow, do not let merge with the trips that start today
            if (currentTrip.DepartureDate.Date == DateTime.Today.AddDays(1))
                mergableTrips = mergableTrips.Where(x => x.DepartureDate.Date != DateTime.Today).ToList();

            if (currentTrip.ReturnDate.Date == DateTime.Today.AddDays(1))
                mergableTrips = mergableTrips.Where(x => x.ReturnDate.Date != DateTime.Today).ToList();

            if (currentTrip.IsPlaneNeeded && currentTrip.PlaneTickets.Count != 0)
                mergableTrips = mergableTrips.Where(x => x.IsPlaneNeeded && (x.PlaneTickets.Count == 0 ||
                                                                             (x.DepartureDate.Date == currentTrip.DepartureDate.Date &&
                                                                              x.ReturnDate.Date == currentTrip.ReturnDate.Date))).ToList();
            

            return mergableTrips;
        }

        public List<object> GetTripDates (int id1, int id2)
        {
            Trip trip1 = tripRepository.Get(id1);
            Trip trip2 = tripRepository.Get(id2);
            object trip1Dates = new {
                DepartureDate = trip1.DepartureDate,
                ReturnDate = trip1.ReturnDate
            };
            object trip2Dates = new
            {
                DepartureDate = trip2.DepartureDate,
                ReturnDate = trip2.ReturnDate
            };
            return new List<object>{trip1Dates, trip2Dates};
        }

        public List<int> GetTripEmployeesIds(int id1, int id2)
        {
            List<int> employeesIds = new List<int>();
            Trip trip = tripRepository.Get(id1);
            trip.EmployeesToTrip.ForEach(x => employeesIds.Add(x.EmployeeID));
            trip = tripRepository.Get(id2);
            trip.EmployeesToTrip.ForEach(x => employeesIds.Add(x.EmployeeID));
            return employeesIds;
        }

        public int MergeTrips(MergeTripsData mergeTripsData)
        {
            Trip tripMergeInto = tripRepository.Get(mergeTripsData.Trip1Id);
            Trip tripMergeFrom = tripRepository.Get(mergeTripsData.Trip2Id);

            //------------------- GAS COMPENSATION ----------------------------------
            //if there are some gas compensations in the merge-from trip
            if (tripMergeFrom.IsCarCompensationNeeded && tripMergeFrom.GasCompensations.Count != 0)
            {
                //if the merge-to trip does not have any gas compensations
                if (!tripMergeInto.IsCarCompensationNeeded)
                {
                    tripMergeInto.IsCarCompensationNeeded = true;
                    tripMergeInto.GasCompensations = new List<GasCompensation>();
                }
                tripMergeInto.GasCompensations.AddRange(tripMergeFrom.GasCompensations);
            }
            else if (tripMergeFrom.IsCarCompensationNeeded) tripMergeInto.IsCarCompensationNeeded = true;

            //------------------- CAR RENTAL ----------------------------------
            if (tripMergeFrom.IsCarRentalNeeded && tripMergeFrom.CarRentals.Count != 0)
            {
                if (!tripMergeFrom.IsCarRentalNeeded)
                {
                    tripMergeInto.IsCarRentalNeeded = true;
                    tripMergeInto.CarRentals = new List<CarRental>();
                }
                tripMergeInto.CarRentals.AddRange(tripMergeFrom.CarRentals);
            }
            else if (tripMergeFrom.IsCarRentalNeeded) tripMergeInto.IsCarRentalNeeded = true;
            
            //------------------- PLANE TICKETS ----------------------------------
            if (tripMergeFrom.IsPlaneNeeded && tripMergeFrom.PlaneTickets.Count != 0)
            {
                if (!tripMergeFrom.IsPlaneNeeded)
                {
                    tripMergeInto.IsPlaneNeeded = true;
                    tripMergeInto.PlaneTickets = new List<PlaneTicket>();
                }
                tripMergeInto.PlaneTickets.AddRange(tripMergeFrom.PlaneTickets);
            }
            else if (tripMergeFrom.IsPlaneNeeded) tripMergeInto.IsPlaneNeeded = true;
            
            //------------------- EMPLOYEES AND RESERVATIONS ----------------------------------
            if (tripMergeInto.DepartureDate.Date != mergeTripsData.departureDate.Date &&
                tripMergeInto.ReturnDate.Date != mergeTripsData.returnDate.Date)
            {
                tripMergeInto.EmployeesToTrip.ForEach(x => x.Status="PENDING");
                tripMergeInto.Reservations = null;
            }
            
            if (tripMergeFrom.DepartureDate.Date != mergeTripsData.departureDate.Date &&
                tripMergeFrom.ReturnDate.Date != mergeTripsData.returnDate.Date)
            {
                tripMergeFrom.EmployeesToTrip.ForEach(x => x.Status="PENDING");
            }
            else
            {
                if (tripMergeFrom.Reservations != null)
                {
                    if (tripMergeInto.Reservations == null) tripMergeInto.Reservations = new List<Reservation>();
                    tripMergeInto.Reservations.AddRange(tripMergeFrom.Reservations);
                }
            }
                tripMergeInto.EmployeesToTrip.AddRange(tripMergeFrom.EmployeesToTrip);

                //------------------- TRIP STATUS ----------------------------------
                
                if (tripMergeInto.EmployeesToTrip.Count(x => x.Status == "APPROVED") > 0)
                    tripMergeInto.Status = "APPROVED";
                else tripMergeInto.Status = "CREATED";
                
                //------------------- TRIP DATES ----------------------------------
                tripMergeInto.DepartureDate = mergeTripsData.departureDate;
                tripMergeInto.ReturnDate = mergeTripsData.returnDate;
                
                //------------------- UPDATE ----------------------------------

                tripRepository.Update(tripMergeInto);
                if(tripRepository.Delete(tripMergeFrom.TripID))
                    return tripMergeInto.TripID;
                return -1;
        }
    }
    
    public class TripMergeDTO
    {
        public int TripID;
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
            TripID = trip.TripID;
            DepartureDate = trip.DepartureDate;
            ReturnDate = trip.ReturnDate;
            this.organiser = organiser;
            EmployeesCount = trip.EmployeesToTrip.Count;
            ConfirmedEmployeesCount = trip.EmployeesToTrip.Count(x => x.Status == "APPROVED");
            BoughtPlaneTicketsCount = trip.IsPlaneNeeded?trip.PlaneTickets.Count:0;
            PlaneTicketsNeeded = trip.IsPlaneNeeded;
            AccomodationCount = trip.Reservations!=null?trip.Reservations.Count:0;
            GasCompensationCount = trip.GasCompensations!=null?trip.GasCompensations.Count:0;
            CarRentalCount = trip.IsCarRentalNeeded?trip.CarRentals.Count:0;
            DepartureOffice = trip.DepartureOffice.City + ", " + trip.DepartureOffice.Country;
            ArrivalOffice = trip.ArrivalOffice.City + ", " + trip.ArrivalOffice.Country;

        }
    }
    
}