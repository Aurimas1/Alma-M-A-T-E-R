using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class Apartment
    {
        public int ApartmentID { get; set; }
        public string Hotel_name { get; set; }    //DevB or 3rd party
        public string Address { get; set; }
        public string Room_number { get; set; }
        public DateTime Check_in { get; set; }
        public DateTime Check_out { get; set; }
        public int Price { get; set; }
        public string Apartment_reservation_url { get; set; }

        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        public int TripID { get; set; }
        public virtual Trip Trip { get; set; }

        public int OfficeID { get; set; }
        public virtual Office Office { get; set; }
    }
}
