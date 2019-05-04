using System;
using System.Collections.Generic;

namespace API.Services
{
    public interface IEventService
    {
        IEnumerable<EventsInformationDTO> GetByDatesAndEmployees(DateTime from, DateTime to, IEnumerable<int> employeeIds);
    }
}