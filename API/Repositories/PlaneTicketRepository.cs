using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class PlaneTicketRepository : IRepository<PlaneTicket>
    {
        private readonly ApiDbContext context;

        public PlaneTicketRepository(ApiDbContext context)
        {
            this.context = context;
        }

        public async Task<PlaneTicket> Add(PlaneTicket item)
        {
            await context.PlaneTickets.AddAsync(item);
            return context.SaveChanges() == 1 ? item : null;
        }

        public bool Delete(int id)
        {
            context.PlaneTickets.Remove(Get(id));
            return context.SaveChanges() == 1;
        }

        public PlaneTicket Get(int id)
        {
            return context.PlaneTickets.FirstOrDefault(x => x.PlaneTicketID == id);
        }

        public PlaneTicket Get(Func<PlaneTicket, bool> predicate)
        {
            return context.PlaneTickets.FirstOrDefault(predicate);
        }

        public IEnumerable<PlaneTicket> GetAll()
        {
            return context.PlaneTickets.ToList();
        }

        public IEnumerable<PlaneTicket> GetAll(Func<PlaneTicket, bool> predicate)
        {
            return context.PlaneTickets.Where(predicate);
        }

        public PlaneTicket Update(PlaneTicket item)
        {
            context.PlaneTickets.Update(item);
            return context.SaveChanges() == 1 ? item : null;
        }
    }
}
