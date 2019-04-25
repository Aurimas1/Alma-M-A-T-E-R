using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API
{
    public class Employee
    {

        [Timestamp]
        public byte[] RowVersion { get; set; }
        
        public virtual List<Reservation> Reservations { get; set; }
  
        public virtual List<GasCompensation> GasCompensations { get; set; }
  
        public virtual List<PlaneTicket> PlaneTickets { get; set; }
        
        public virtual List<Event> Events { get; set; }
        
        public virtual List<EmployeeToTrip> EmployeeToTrip { get; set; }
         
    }
}
