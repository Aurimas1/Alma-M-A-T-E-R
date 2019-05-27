using API.Models;
using System.Collections.Generic;
using System.Linq;

namespace API.Extensions
{
    public static class GasCompensationExtensions
    {
        public static IEnumerable<GasCompensationInfo> ToInfo(this List<GasCompensation> gas)
        {
            return gas.Select(x => new GasCompensationInfo
            {
                Id = x.GasCompensationID,
                Name = x.Employee.Name,
                Currency = x.Currency,
                Price = x.Price,
                EmployeeID = x.EmployeeID,
            });
        }
    }
}
