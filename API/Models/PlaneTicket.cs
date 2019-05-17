using System;

namespace API
{
    public class PlaneTicket
    {
        public int PlaneTicketID { get; set; }
        public string FlightCompany { get; set; }
        public string Airport { get; set; }
        public DateTime ForwardFlightDate { get; set; }
        public DateTime ReturnFlightDate { get; set; }
        public int Price { get; set; }
        public string PlaneTicketUrl { get; set; }
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
        public int TripID { get; set; }
        public virtual Trip Trip { get; set; }
        public string Currency { get; set; }
    }
}
