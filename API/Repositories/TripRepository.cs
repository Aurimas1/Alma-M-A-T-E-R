using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class TripRepository : IRepository<Trip>
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
            return context.Trips
                .Include(x => x.EmployeesToTrip).ThenInclude(x => x.Employee)
                .Include(x => x.Reservations).ThenInclude(x => x.Apartment)
                .Include(x => x.ArrivalOffice)
                .Include(x => x.ArrivalOffice.Apartaments)
                .Include(x => x.DepartureOffice)
                .Include(x => x.PlaneTickets)
                .Include(x => x.CarRentals)
                .Include(x => x.GasCompensations).ThenInclude(x => x.Employee)
                .FirstOrDefault(x => x.TripID == id);
        }

        public Trip Get(Func<Trip, bool> predicate)
        {
            return context.Trips.FirstOrDefault(predicate);
        }

        public IEnumerable<Trip> GetAll()
        {
            return context.Trips
                .Include(x => x.EmployeesToTrip).ThenInclude(x => x.Employee)
                .Include(x => x.Reservations)
                .Include(x => x.ArrivalOffice)
                .Include(x => x.ArrivalOffice.Apartaments)
                .Include(x => x.PlaneTickets)
                .Include(x => x.CarRentals)
                .ToList();
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
    }
}
