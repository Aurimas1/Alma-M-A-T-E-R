using System;

namespace API
{
    public class Event
    {
        public int EventID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string name { get; set; }
        
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
    }
}