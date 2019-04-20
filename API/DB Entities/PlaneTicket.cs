using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class PlaneTicket
    {
        public int PlaneTicketID { get; set; }
        public string FlightCompany { get; set; }
        public string Airport { get; set; }
        public DateTime ForwardFlightFate { get; set; }
        public DateTime ReturnFlightFate { get; set; }
        public int Price { get; set; }
        public string PlaneTicketUrl { get; set; }

        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        public int TripID { get; set; }
        public virtual Trip Trip { get; set; }
    }
}
