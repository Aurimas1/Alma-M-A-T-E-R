using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API
{
    public class Trip
    {
        public int TripID { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Status { get; set; } //CREATED, APPROVED, COMPLETED
        public bool IsPlaneNeeded { get; set; } 
        public bool IsCarRentalNeeded { get; set; }
        public bool IsCarCompensationNeeded { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
        public int DepartureOfficeID { get; set; }
        public virtual Office DepartureOffice { get; set; }
        public int ArrivalOfficeID { get; set; }
        public virtual Office ArrivalOffice { get; set; }
        //Problem with two foreign keys to the same table?
        //https://stackoverflow.com/questions/5559043/entity-framework-code-first-two-foreign-keys-from-same-table

        public virtual List<Reservation> Reservations { get; set; }
        public virtual List<PlaneTicket> PlaneTickets { get; set; }
        public virtual List<CarRental> CarRentals { get; set; }
        public virtual List<GasCompensation> GasCompensations { get; set; }
        public virtual List<EmployeeToTrip> EmployeesToTrip { get; set; }

    }
}
