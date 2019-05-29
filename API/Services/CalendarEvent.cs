using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class CalendarEvent
    {
        public string Summary { get; set; }
        public string Location { get; set; }
        public CalendarTimes Start { get; set; }
        public CalendarTimes End { get; set; }
    }

    public class CalendarTimes
    {
        public string Date { get; set; }
        public string DateTime { get; set; }
    }

    public class CalendarResponse
    {
        public ICollection<CalendarEvent> Items { get; set; }
    }

    public static class CalendarExtensions
    {
        public static IEnumerable<Event> ToEvents(this IEnumerable<CalendarEvent> events, int employeeId)
        {
            return events.Select(@event => new Event
            {
                DateFrom = DateTime.Parse(@event.Start.Date ?? @event.Start.DateTime),
                DateTo = DateTime.Parse(@event.End.Date ?? @event.Start.DateTime),
                name = @event.Summary,
                EmployeeID = employeeId,
            });

        }
    }
}
