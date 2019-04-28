using System;

namespace API
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string ReservationUrl { get; set; }
        public int ApartmentID { get; set; }
        public virtual Apartment Apartment { get; set; }
        public int TripID { get; set; }
        public virtual Trip Trip { get; set; }
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
