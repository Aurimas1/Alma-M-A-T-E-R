using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class Office
    {
        public int OfficeID { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        public virtual List<Apartment> Apartaments { get; set; }
    }
}
