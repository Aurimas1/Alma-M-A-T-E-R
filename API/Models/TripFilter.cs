using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class TripFilter
    {
        public int ID { get; set; }
        public Office ArrivalOffice { get; set; }
        public Office DepartureOffice { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public double ConfirmedProcentage { get; set; }
        public string EmployeeCount { get; set; }
        public double AccomodationProcentage { get; set; }
        public string AccomodationCount { get; set; }
        public double PlaneTicketProcentage { get; set; }
        public string PlaneTicketCount { get; set; }
        public double CarRentalProcentage { get; set; }
        public string CarRentalCount { get; set; }
        public string Status { get; set; }
        public IEnumerable<string> EmployeeEmailList { get; set; }
    }
}
