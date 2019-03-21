using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class Plane_ticket
    {
        public int Plane_ticketID { get; set; }
        public string Flight_company { get; set; }
        public string Airport { get; set; }
        public DateTime Forward_flight_date { get; set; }
        public DateTime Return_flight_date { get; set; }
        public string Price { get; set; }
        public string Plane_ticket_url { get; set; }

        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        public int TripID { get; set; }
        public virtual Trip Trip { get; set; }
    }
}
