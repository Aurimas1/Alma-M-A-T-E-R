using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API
{
    public class Apartment
    {
        public int ApartmentID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int RoomNumber { get; set; }
        public int Price { get; set; }
        public string Type { get; set; } //OFFICE or HOTEL or HOME
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public int? OfficeId { get; set; }
        [ForeignKey("OfficeId")]
        public Office Office { get; set; }
        public virtual List<Reservation> Reservations { get; set; }
        public string Currency { get; set; }
    }
}