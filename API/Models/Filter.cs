using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Filter
    {
        public bool TripsAwaitingConfirmation { get; set; }
        public bool TripsConfirmed { get; set; }
        public bool FullyPlannedTrips { get; set; }
        public bool FinishedTrips { get; set; }
        public bool MyOrganizedTrips { get; set; }
        public bool OtherOrganizedTrips { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
