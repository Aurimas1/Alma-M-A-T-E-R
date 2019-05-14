using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class TripRepository
    {
        private readonly ApiDbContext context;

        public TripRepository(ApiDbContext context)
        {
            this.context = context;
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
            return context.Trips.Include(x => x.EmployeesToTrip).Include(x => x.Reservations).FirstOrDefault(x => x.TripID == id);
        }

        public Trip Get(Func<Trip, bool> predicate)
        {
            return context.Trips.FirstOrDefault(predicate);
        }

        public IEnumerable<Trip> GetAll()
        {
            return context.Trips.ToList();
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
