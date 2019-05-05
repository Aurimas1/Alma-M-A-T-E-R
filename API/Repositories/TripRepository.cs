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
            return context.Trips.FirstOrDefault(x => x.TripID == id);
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

        public IEnumerable<Employee> GetEmployeesFromTrip(int id)
        {
            List<Employee> employees = context.EmployeeToTrips
                .Where(x => x.TripId == id)
                .Select(x => x.Employee)
                .Distinct()
                .ToList();
            return employees;
        }
    }
}
