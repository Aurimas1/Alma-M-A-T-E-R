using System;

namespace API
{
    public class CarRental
    {
        public int CarRentalID { get; set; }
        public string CarRentalCompany { get; set; }
        public string CarPickupAddress { get; set; }
        public DateTime CarIssueDate { get; set; }
        public DateTime CarReturnDate { get; set; }
        public int Price { get; set; }
        public string CarRentalUrl { get; set; }   
        public int TripID { get; set; }
        public virtual Trip Trip { get; set; }
    }
}
