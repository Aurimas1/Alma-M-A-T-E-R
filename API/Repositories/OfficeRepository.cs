using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class OfficeRepository : IRepository<Office>
    {
        private readonly ApiDbContext context;

        public OfficeRepository(ApiDbContext context)
        {
            this.context = context;
        }

        public async Task<Office> Add(Office item)
        {
            await context.Offices.AddAsync(item);
            return context.SaveChanges() == 1 ? item : null;
        }

        public bool Delete(int id)
        {
            context.Offices.Remove(Get(id));
            return context.SaveChanges() == 1;
        }

        public Office Get(int id)
        {
            return context.Offices.FirstOrDefault(x => x.OfficeID == id);
        }

        public Office Get(Func<Office, bool> predicate)
        {
            return context.Offices.FirstOrDefault(predicate);
        }

        public IEnumerable<Office> GetAll()
        {
            return context.Offices.ToList();
        }

        public IEnumerable<Office> GetAll(Func<Office, bool> predicate)
        {
            return context.Offices.Where(predicate);
        }

        public Office Update(Office item)
        {
            context.Offices.Update(item);
            return context.SaveChanges() == 1 ? item : null;
        }
    }
}
