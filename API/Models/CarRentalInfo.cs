using System;

namespace API.Models
{
    public class CarRentalInfo
    {
        public int Id { get; set; }
        public string CarRentalCompany { get; set; }
        public string CarPickupAddress { get; set; }
        public DateTime CarIssueDate { get; set; }
        public DateTime CarReturnDate { get; set; }
        public int Price { get; set; }
        public string CarRentalUrl { get; set; }
        public string Currency { get; set; }
    }
}
