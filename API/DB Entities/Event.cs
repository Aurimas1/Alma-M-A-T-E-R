using System;
using System.ComponentModel.DataAnnotations;

namespace API
{
    public class Event
    {
        public int EventID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string name { get; set; }
        
        [Timestamp]
        public byte[] RowVersion { get; set; }
        
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
    }
}