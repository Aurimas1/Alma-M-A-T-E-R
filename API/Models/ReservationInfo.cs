using System;

namespace API
{
    public class ReservationInfo
    {
        public int ReservationID { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string ReservationUrl { get; set; }
        public string EmployeeName { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int RoomNumber { get; set; }
        public int Price { get; set; }
        public string Currency { get; set; }
        public string Type { get; set; }
        public int EmployeeID { get; set; }
    }
}
