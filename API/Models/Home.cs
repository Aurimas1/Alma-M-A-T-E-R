using System;

namespace API
{
    public class Home
    {
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int TripID { get; set; }
        public int EmployeeID { get; set; }
        public string Address { get; set; }
        public string Currency { get; set; }
    }
}
