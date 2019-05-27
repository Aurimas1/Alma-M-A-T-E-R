using API.Models;
using System.Collections.Generic;
using System.Linq;

namespace API.Extensions
{
    public static class ReservationlExtensions
    {
        public static IEnumerable<ReservationInfo> ToInfo(this List<Reservation> reservations)
        {
            return reservations.Select(r => new ReservationInfo
            {
                Type = r.Apartment.Type,
                ReservationID = r.ReservationID,
                Address = r.Apartment.Address,
                CheckIn = r.CheckIn,
                CheckOut = r.CheckOut,
                Currency = r.Apartment.Currency,
                EmployeeName = r.Employee.Name,
                Name = r.Apartment.Name,
                Price = r.Apartment.Price,
                ReservationUrl = r.ReservationUrl,
                RoomNumber = r.Apartment.RoomNumber,
                EmployeeID = r.EmployeeID,
            });
        }
    }
}
