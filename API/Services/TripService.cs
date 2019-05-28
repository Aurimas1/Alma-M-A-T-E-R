using API.Extensions;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class TripService : ITripService
    {
        private readonly IRepository<Trip> repository;
        private readonly IRepository<GasCompensation> gasCompensationRepository;
        private readonly IRepository<CarRental> carRentalRepository;
        private readonly IRepository<PlaneTicket> planeTicketRepository;
        private readonly IRepository<Apartment> apartmentRepository;
        private readonly IRepository<Reservation> reservationRepository;
        private readonly IRepository<EmployeeToTrip> empToTripRepository;
        private readonly IHttpContextAccessor accessor;

        public TripService(IRepository<Trip> repository, IRepository<GasCompensation> gasCompensationRepository, IRepository<CarRental> carRentalRepository, IRepository<PlaneTicket> planeTicketRepository, IRepository<Apartment> apartmentRepository, IRepository<Reservation> reservationRepository, IRepository<EmployeeToTrip> empToTripRepository, IHttpContextAccessor accessor)
        {
            this.repository = repository;
            this.gasCompensationRepository = gasCompensationRepository;
            this.carRentalRepository = carRentalRepository;
            this.planeTicketRepository = planeTicketRepository;
            this.apartmentRepository = apartmentRepository;
            this.reservationRepository = reservationRepository;
            this.empToTripRepository = empToTripRepository;
            this.accessor = accessor;
        }

        public async Task<GasCompensation> SaveGasCompensation(GasCompensation item)
        {
            return await gasCompensationRepository.Add(item);
        }

        public async Task<CarRental> SaveCarRental(CarRental item)
        {
            return await carRentalRepository.Add(item);
        }

        public async Task<PlaneTicket> SavePlaneTicket(PlaneTicket item)
        {
            return await planeTicketRepository.Add(item);
        }

        public async Task<Apartment> SaveHotelorHome(Apartment item)
        {
            return await apartmentRepository.Add(item);
        }

        public bool Delete(int id)
        {
            return repository.Delete(id);
        }

        public async Task<Reservation> SaveReservation(Reservation item)
        {
            return await reservationRepository.Add(item);
        }

        public async Task<Trip> Add(Trip item)
        {
            var empToTrip = item.EmployeesToTrip;
            item.EmployeesToTrip = new List<EmployeeToTrip>();
            var result = await repository.Add(item);

            foreach (var employee in empToTrip)
            {
                var employeeToTrip = new EmployeeToTrip
                {
                    EmployeeID = employee.EmployeeID,
                    TripId = result.TripID,
                    Status = "PENDING",
                    WasRead = false,
                };
                await empToTripRepository.Add(employeeToTrip);
            }

            return result;
        }

        public Trip Update(Trip item)
        {
            return repository.Update(item);
        }

        public IEnumerable<Trip> GetAll()
        {
            return repository.GetAll();
        }

        public IEnumerable<Trip> GetAllMyTrips()
        {
            return repository.GetAll(x => x.EmployeesToTrip.Select(a => a.EmployeeID == accessor.HttpContext.User.GetEmpoeeID()).Contains(true));
        }

        public Trip GetByID(int id)
        {
            return repository.Get(id);
        }

        public TimeAndTransport GetTimeAndTransport(int id)
        {
            var trip = repository.Get(id);
            return new TimeAndTransport()
            {
                DepartureDate = trip.DepartureDate,
                ReturnDate = trip.ReturnDate,
                IsCarCompensationNeeded = trip.IsCarCompensationNeeded,
                IsCarRentalNeeded = trip.IsCarRentalNeeded,
                IsPlaneNeeded = trip.IsPlaneNeeded,
            };
        }

        public IEnumerable<EmployeeWithStatus> GetEmployees(int id) // need testing
        {
            return repository.Get(id).EmployeesToTrip.Select(x => new EmployeeWithStatus(x));
        }

        public IEnumerable<Apartment> GetReservedApartments(int id)
        {
            return repository.Get(id).Reservations.Select(x => x.Apartment); // need testing
        }

        public IEnumerable<PlaneTicket> GetPlaneTickets(int id)
        {
            return repository.Get(id).PlaneTickets;
        }

        public IEnumerable<CarRental> GetCarRentals(int id)
        {
            return repository.Get(id).CarRentals;
        }

        public IEnumerable<GasCompensation> GetGasCompensations(int id)
        {
            return repository.Get(id).GasCompensations;
        }

        public Trip Get(int id)
        {
            return repository.Get(id);
        }

        public IEnumerable<Trip> GetYourOrganizedTrips() {
            return repository.GetAll(x => x.OrganizerID == accessor.HttpContext.User.GetEmpoeeID());
        }

        public IEnumerable<Trip> GetOtherOrganizedTrips()
        {
            return repository.GetAll(x => x.OrganizerID != accessor.HttpContext.User.GetEmpoeeID());
        }
    }
}
