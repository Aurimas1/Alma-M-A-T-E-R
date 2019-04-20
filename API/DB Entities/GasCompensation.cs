using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class GasCompensation
    {
        public int GasCompensationID { get; set; }
        public int Price { get; set; }

        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        public int TripID { get; set; }
        public virtual Trip Trip { get; set; }
    }
}
