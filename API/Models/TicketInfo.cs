using System;

namespace API.Models
{
    public class TicketInfo
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string FlightCompany { get; set; }
        public string Airport { get; set; }
        public DateTime ForwardFlightDate { get; set; }
        public DateTime ReturnFlightDate { get; set; }
        public string PlaneTicketUrl { get; set; }
        public int EmployeeID { get; set; }
        public int Price { get; set; }
        public string Currency { get; set; }
    }
}
