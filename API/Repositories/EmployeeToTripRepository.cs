using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class EmployeeToTripRepository : IRepository<EmployeeToTrip>
    {
        private readonly ApiDbContext context;

        public EmployeeToTripRepository(ApiDbContext context)
        {
            this.context = context;
        }

        public async Task<EmployeeToTrip> Add(EmployeeToTrip item)
        {
            await context.EmployeeToTrips.AddAsync(item);
            return context.SaveChanges() == 1 ? item : null;
        }

        public bool Delete(int id)
        {
            context.EmployeeToTrips.Remove(Get(id));
            return context.SaveChanges() == 1;
        }

        public EmployeeToTrip Get(int id)
        {
            return context.EmployeeToTrips.FirstOrDefault(x => x.EmployeeToTripID == id);
        }

        public EmployeeToTrip Get(Func<EmployeeToTrip, bool> predicate)
        {
            return context.EmployeeToTrips.FirstOrDefault(predicate);
        }

        public IEnumerable<EmployeeToTrip> GetAll()
        {
            return context.EmployeeToTrips.ToList();
        }

        public IEnumerable<EmployeeToTrip> GetAll(Func<EmployeeToTrip, bool> predicate)
        {
            return context.EmployeeToTrips.Where(predicate);
        }

        public EmployeeToTrip Update(EmployeeToTrip item)
        {
            context.EmployeeToTrips.Update(item);
            return context.SaveChanges() == 1 ? item : null;
        }
    }
}
