using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class GasCompensationRepository : IRepository<GasCompensation>
    {
        private readonly ApiDbContext context;

        public GasCompensationRepository(ApiDbContext context)
        {
            this.context = context;
        }
        
        public async Task<GasCompensation> Add(GasCompensation item)
        {
            await context.GasCompensations.AddAsync(item);
            return context.SaveChanges() == 1 ? item : null;
        }

        public IEnumerable<GasCompensation> GetAll()
        {
            return context.GasCompensations.ToList();
        }

        public IEnumerable<GasCompensation> GetAll(Func<GasCompensation, bool> predicate)
        {
            return context.GasCompensations.ToList().Where(predicate);
 
        }

        public GasCompensation Get(int id)
        {
            return context.GasCompensations.FirstOrDefault(x => x.GasCompensationID == id);
        }

        public GasCompensation Get(Func<GasCompensation, bool> predicate)
        {
            return context.GasCompensations.FirstOrDefault(predicate);
        }

        public GasCompensation Update(GasCompensation item)
        {
            context.GasCompensations.Update(item);
            return context.SaveChanges() == 1 ? item : null;
        }

        public bool Delete(int id)
        {
            context.GasCompensations.Remove(Get(id));
            return context.SaveChanges() == 1;
        }
    }
}