using System.ComponentModel.DataAnnotations;

namespace API
{
    public class GasCompensation
    {
        public int GasCompensationID { get; set; }
        public int Price { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
        
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        public int TripID { get; set; }
        public virtual Trip Trip { get; set; }
    }
}
