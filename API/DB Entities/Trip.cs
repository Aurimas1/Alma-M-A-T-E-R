using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class Trip
    {
        public int TripID { get; set; }
        public DateTime Departure_date { get; set; }
        public DateTime Arrival_date { get; set; }

        public List<int> Plane_tickets { get; set; }
        public virtual List<Plane_ticket> Plane_Tickets { get; set; }

        public List<int> Car_rentalsID { get; set; }
        public virtual List<Car_rental> Car_rentals { get; set; }

        public List<int> Gas_compensationID { get; set; }
        public virtual List<Gas_compensation> Gas_compensations { get; set; }

        public List<int> ApartmentsID { get; set; }
        public virtual List<Apartment> Apartments { get; set; }

        public int Departure_officeID { get; set; }
        public virtual Office Departure_office { get; set; }

        public int Arrival_officeID { get; set; }
        public virtual Office Arrival_office { get; set; }

        public List<int> EmployeesID { get; set; }
        public virtual List<Employee> Employees { get; set; }
    }
}
