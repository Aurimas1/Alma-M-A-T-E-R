using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class ApartmentRepository : IRepository<Apartment>
    {
        private readonly ApiDbContext context;

        public ApartmentRepository(ApiDbContext context)
        {
            this.context = context;
        }
        public async Task<Apartment> Add(Apartment item)
        {
            await context.Apartments.AddAsync(item);
            return context.SaveChanges() == 1 ? item : null;
        }

        public bool Delete(int id)
        {
            context.Apartments.Remove(Get(id));
            return context.SaveChanges() == 1;
        }

        public Apartment Get(int id)
        {
            return context.Apartments.Include(x => x.Reservations).FirstOrDefault(x => x.ApartmentID == id);
        }

        public Apartment Get(Func<Apartment, bool> predicate)
        {
            return context.Apartments.FirstOrDefault(predicate);
        }

        public IEnumerable<Apartment> GetAll()
        {
            return context.Apartments.ToList();
        }

        public IEnumerable<Apartment> GetAll(Func<Apartment, bool> predicate)
        {
            return context.Apartments.Where(predicate);
        }

        public Apartment Update(Apartment item)
        {
            context.Apartments.Update(item);
            return context.SaveChanges() == 1 ? item : null;
        }
    }
}
