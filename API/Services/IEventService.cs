using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IEventService
    {
        Task SaveEventsForEmployee(IEnumerable<Event> events);
    }
}
