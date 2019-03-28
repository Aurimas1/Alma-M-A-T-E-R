using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone_number { get; set; }
        public string Email_address { get; set; }

        public List<int> TripsID { get; set; }
        public virtual List<Trip> Trips { get; set; }

        public virtual Apartment Apartment { get; set; }

        public virtual Gas_compensation Gas_compensation { get; set; }

        public virtual Plane_ticket Plane_ticket { get; set; }
    }
}
