using API.Models;
using System.Collections.Generic;
using System.Linq;

namespace API.Extensions
{
    public static class CarRentalExtensions
    {
        public static IEnumerable<CarRentalInfo> ToInfo(this List<CarRental> rentals)
        {
            return rentals.Select(r => new CarRentalInfo
            {
                CarIssueDate = r.CarIssueDate,
                CarPickupAddress = r.CarPickupAddress,
                CarRentalCompany = r.CarRentalCompany,
                CarRentalUrl = r.CarRentalUrl,
                CarReturnDate = r.CarReturnDate,
                Currency = r.Currency,
                Price = r.Price,
            });
        }
    }
}
