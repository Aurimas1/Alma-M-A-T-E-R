using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class TripRepository
    {
        private readonly ApiDbContext context;
        private readonly IRepository<Office> officeContext;
        private readonly IRepository<EmployeeToTrip> employeeToTripContext;
        private readonly IRepository<Reservation> reservationContext;
        private readonly IRepository<PlaneTicket> planeTicketContext;
        private readonly IRepository<CarRental> carRentalContext;
        private readonly IRepository<Employee> employeeContext;
        private readonly IRepository<GasCompensation> gasCompensationContext;

        public TripRepository(ApiDbContext context, IRepository<Office> officeContext, IRepository<EmployeeToTrip> employeeToTripContext, IRepository<Reservation> reservationContext, IRepository<PlaneTicket> planeTicketContext, IRepository<CarRental> carRentalContext, IRepository<Employee> employeeContext, IRepository<GasCompensation> gasCompensationContext)
        {
            this.context = context;
            this.officeContext = officeContext;
            this.employeeToTripContext = employeeToTripContext;
            this.reservationContext = reservationContext;
            this.planeTicketContext = planeTicketContext;
            this.carRentalContext = carRentalContext;
            this.employeeContext = employeeContext;
            this.gasCompensationContext = gasCompensationContext;
        }

        public async Task<Trip> Add(Trip item)
        {
            await context.Trips.AddAsync(item);
            return context.SaveChanges() == 1 ? item : null;
        }

        public bool Delete(int id)
        {
            context.Trips.Remove(Get(id));
            return context.SaveChanges() == 1;
        }

        public Trip Get(int id)
        {
            var offices = officeContext.GetAll();
            var reservations = reservationContext.GetAll();
            var employeeToTrips = employeeToTripContext.GetAll();
            var planeTickets = planeTicketContext.GetAll();
            var carRentals = carRentalContext.GetAll();
            var employees = employeeContext.GetAll();
            var gasCompensations = gasCompensationContext.GetAll();

            return context.Trips.FirstOrDefault(x => x.TripID == id);
        }

        public Trip Get(Func<Trip, bool> predicate)
        {
            return context.Trips.FirstOrDefault(predicate);
        }

        public IEnumerable<Trip> GetAll()
        {
            var trips = context.Trips.ToList();
            var reservations = reservationContext.GetAll();
            var employeeToTrips = employeeToTripContext.GetAll();
            var planeTickets = planeTicketContext.GetAll();
            var carRentals = carRentalContext.GetAll();
            var employees = employeeContext.GetAll();

            foreach(var trip in trips)
            {
                trip.ArrivalOffice = officeContext.Get(trip.ArrivalOfficeID);
                trip.DepartureOffice = officeContext.Get(trip.DepartureOfficeID);
            }
            return trips;
        }

        public IEnumerable<Trip> GetAll(Func<Trip, bool> predicate)
        {

            return context.Trips.Where(predicate);
        }

        public Trip Update(Trip item)
        {
            context.Trips.Update(item);
            return context.SaveChanges() == 1 ? item : null;
        }

        public IEnumerable<Employee> GetEmployees(int id)
        {
            List<Employee> employees = context.EmployeeToTrips
                .Where(x => x.TripId == id)
                .Select(x => x.Employee)
                .ToList();
            return employees;
        }

        public IEnumerable<Apartment> GetReservedApartments(int id)
        {
            List<Apartment> apartments = context.Reservations
                .Where(x => x.TripID == id)
                .Select(x => x.Apartment)
                .ToList();
            return apartments;
        }

        public IEnumerable<PlaneTicket> GetPlaneTickets(int id)
        {
            List<PlaneTicket> planeTickets = context.PlaneTickets
                .Where(x => x.TripID == id)
                .ToList();
            return planeTickets;
        }

        public IEnumerable<CarRental> GetCarRentals(int id)
        {
            List<CarRental> carRentals = context.CarRentals
                .Where(x => x.TripID == id)
                .ToList();
            return carRentals;
        }

        public IEnumerable<GasCompensation> GetGasCompensations(int id)
        {
            List<GasCompensation> gasCompensations = context.GasCompensations
                .Where(x => x.TripID == id)
                .ToList();
            return gasCompensations;
        }
    }
}
