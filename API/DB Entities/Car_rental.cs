using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class Car_rental
    {
        public int Car_rentalID { get; set; }
        public string Car_rental_company { get; set; }
        public string Car_pickup_address { get; set; }
        public DateTime Car_issue_date { get; set; }
        public DateTime Car_return_date { get; set; }
        public string Price { get; set; }
        public string Car_rental_url { get; set; }

        public int TripID { get; set; }
        public virtual Trip Trip { get; set; }
    }
}
