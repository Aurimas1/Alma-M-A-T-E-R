using System;
using System.Collections.Generic;

namespace API
{
    public class CreateTrip
    {
        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool IsPlaneNeeded { get; set; }
        public bool IsCarRentalNeeded { get; set; }
        public bool IsCarCompensationNeeded { get; set; }
        public int DepartureOfficeID { get; set; }
        public int ArrivalOfficeID { get; set; }
        public IEnumerable<int> Employees { get; set; }
    }
}
