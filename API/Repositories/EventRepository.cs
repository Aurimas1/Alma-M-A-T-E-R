using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class EventRepository : IRepository<Event>
    {
        private readonly ApiDbContext context;

        public EventRepository(ApiDbContext context)
        {
            this.context = context;
        }

        public async Task<Event> Add(Event item)
        {
            await context.Events.AddAsync(item);
            return context.SaveChanges() == 1 ? item : null;
        }


        public bool Delete(int id)
        {
            context.Events.Remove(Get(id));
            return context.SaveChanges() == 1;
        }

        public Event Get(int id)
        {
            return context.Events.FirstOrDefault(x => x.EventID == id);
        }

        public Event Get(Func<Event, bool> predicate)
        {
            return context.Events.FirstOrDefault(predicate);
        }

        public IEnumerable<Event> GetAll()
        {
            return context.Events.ToList();
        }

        public IEnumerable<Event> GetAll(Func<Event, bool> predicate)
        {
            return context.Events.Where(predicate);
        }

        public Event Update(Event item)
        {
            context.Events.Update(item);
            return context.SaveChanges() == 1 ? item : null;
        }
    }
}
