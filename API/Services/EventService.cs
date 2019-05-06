using API.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public class EventService : IEventService
    {
        private readonly IRepository<Event> repository;

        public EventService(IRepository<Event> repository)
        {
            this.repository = repository;
        }

        public async Task SaveEventsForEmployee(IEnumerable<Event> events)
        {
            foreach (var @event in events)
                await repository.Add(@event);
        }
    }
}
