using System.Collections.Generic;

namespace API
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public virtual List<Reservation> Reservations { get; set; }
        public virtual List<GasCompensation> GasCompensations { get; set; }
        public virtual List<PlaneTicket> PlaneTickets { get; set; }
        public virtual List<Event> Events { get; set; }
        public virtual List<EmployeeToTrip> EmployeeToTrip { get; set; }
    }
}
