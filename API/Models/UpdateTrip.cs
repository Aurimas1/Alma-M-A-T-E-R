using System;
using System.Collections.Generic;

namespace API
{
    public class UpdateTrip
    {
        public bool IsPlaneNeeded { get; set; }
        public bool IsCarRentalNeeded { get; set; }
        public bool IsCarCompensationNeeded { get; set; }
        public IEnumerable<int> Employees { get; set; }
        public IEnumerable<Room> Rooms { get; set; }
    }

    public class Room
    {
        public int RoomID { get; set; }
        public int EmployeeID { get; set; }
    }
}
