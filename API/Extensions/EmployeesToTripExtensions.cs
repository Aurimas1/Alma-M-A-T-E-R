using API.Models;
using System.Collections.Generic;
using System.Linq;

namespace API.Extensions
{
    public static class EmployeesToTripExtensions
    {
        public static IEnumerable<EmployeeToTripInfo> ToInfo(this List<EmployeeToTrip> list)
        {
            return list.Select(x => new EmployeeToTripInfo
            {
                EmployeeName = x.Employee.Name,
                EmployeeEmail = x.Employee.Email,
                EmployeeID = x.Employee.EmployeeID,
                EmployeeStatus = x.Status,
            });
        }
    }
}
