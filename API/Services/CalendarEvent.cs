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
    }

    public class CalendarResponse
    {
        public ICollection<CalendarEvent> Items { get; set; }
    }
}
