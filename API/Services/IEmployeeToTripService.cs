using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IEmployeeToTripService
    {
        Task<EmployeeToTrip> Add(EmployeeToTrip item);
        bool Remove(int id);
    }
}
