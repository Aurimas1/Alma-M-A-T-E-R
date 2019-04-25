using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace API
{
    public class Office
    {
        public int OfficeID { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
        
        public virtual List<Apartment> Apartaments { get; set; }
    }
}
