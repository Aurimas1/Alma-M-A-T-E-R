using API.Models;
using System.Collections.Generic;
using System.Linq;

namespace API.Extensions
{
    public static class TicketExtensions
    {
        public static IEnumerable<TicketInfo> ToInfo(this List<PlaneTicket> ticket)
        {
            return ticket.Select(t => new TicketInfo
            {
                Id = t.PlaneTicketID,
                Airport = t.Airport,
                Currency = t.Currency,
                EmployeeID = t.EmployeeID,
                FlightCompany = t.FlightCompany,
                ForwardFlightDate = t.ForwardFlightDate,
                Price = t.Price,
                PlaneTicketUrl = t.PlaneTicketUrl,
                ReturnFlightDate = t.ReturnFlightDate,
                EmployeeName = t.Employee.Name,
            });
        }
    }
}
