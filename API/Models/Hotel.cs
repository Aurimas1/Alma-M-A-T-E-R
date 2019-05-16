using System;

namespace API
{
    public class Hotel
    {
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string ReservationUrl { get; set; }
        public int TripID { get; set; }
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int RoomNumber { get; set; }
        public int Price { get; set; }
    }
}
