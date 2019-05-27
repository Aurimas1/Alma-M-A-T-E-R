using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class GasCompensationInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Currency { get; set; }
        public int EmployeeID { get; set; }
    }
}
