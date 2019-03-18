using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class Apartment
    {
        public int ApartmentID { get; set; }
        public string Company { get; set; }    //DevB or 3rd party
        public string Address { get; set; }
        public string Room_number { get; set; }
        public DateTime Arrival_date { get; set; }
        public DateTime Departure_date { get; set; }
        public int Price { get; set; }

        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        public int TripID { get; set; }
        public virtual Trip Trip { get; set; }
    }
}
