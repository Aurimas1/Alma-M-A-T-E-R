using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Extensions
{
    public static class TripExtensions
    {
        public static IEnumerable<TripFilter> ToTripFilter(this IEnumerable<Trip> trips) 
        {
            return trips.Select(t =>
            {

                double confirmedProcentage = 0;
                var employees = 0;
                var employeeCount = "0/0";

                double accomodationProcentage = 0;
                var accomodationCount = "0/0";

                double planeTicketProcentage = 0;
                var planeTicketCount = "0/0";

                double carRentalProcentage = 0;
                var carRentalCount = "0";

                employees = 0;
                foreach (var i in t.EmployeesToTrip)
                {
                    if (i.Status == "APPROVED")
                    {
                        employees++;
                    }
                }

                confirmedProcentage = ((double)employees / (double)t.EmployeesToTrip.Count);
                confirmedProcentage = Math.Round(confirmedProcentage, 1, MidpointRounding.AwayFromZero) * 100;
                employeeCount = employees + "/" + t.EmployeesToTrip.Count;

                accomodationProcentage = (double)(t.Reservations?.Count ?? 0) / (double)t.EmployeesToTrip.Count;
                accomodationProcentage = Math.Round(accomodationProcentage, 1, MidpointRounding.AwayFromZero) * 100;


                accomodationCount = (t.Reservations?.Count ?? 0) + "/" + t.EmployeesToTrip.Count;

                if (t.IsPlaneNeeded)
                {
                    planeTicketProcentage = (double)(t.PlaneTickets?.Count ?? 0) / (double)t.EmployeesToTrip.Count;
                    planeTicketProcentage = Math.Round(planeTicketProcentage, 1, MidpointRounding.AwayFromZero) * 100;
                    planeTicketCount = (t.PlaneTickets?.Count ?? 0) + "/" + t.EmployeesToTrip.Count;
                }
                else planeTicketProcentage = 100;

                if (t.IsCarRentalNeeded)
                {
                    if((t.CarRentals?.Count ?? 0) > 0)
                    {
                        carRentalProcentage = 100;
                    }
                    carRentalCount = (t.CarRentals?.Count ?? 0).ToString();
                }
                else carRentalProcentage = 100;

                return new TripFilter
                {
                    ID = t.TripID,
                    ArrivalOffice = t.ArrivalOffice,
                    DepartureOffice = t.DepartureOffice,
                    ReturnDate = t.ReturnDate,
                    DepartureDate = t.DepartureDate,
                    ConfirmedProcentage = confirmedProcentage,
                    EmployeeCount = employeeCount,
                    AccomodationProcentage = accomodationProcentage,
                    AccomodationCount = accomodationCount,
                    PlaneTicketProcentage = planeTicketProcentage,
                    PlaneTicketCount = planeTicketCount,
                    CarRentalProcentage = carRentalProcentage,
                    CarRentalCount = carRentalCount,
                    Status = t.Status,
                };
            });
        }
    }
}
