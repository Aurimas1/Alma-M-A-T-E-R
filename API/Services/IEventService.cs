using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IEventService
    {
        IEnumerable<EventsInformationDTO> GetByDatesAndEmployees(DateTime from, DateTime to, IEnumerable<int> employeeIds);
        Task SaveEventsForEmployee(IEnumerable<Event> events);
    }
}
