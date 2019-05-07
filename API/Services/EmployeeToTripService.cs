using API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class EmployeeToTripService : IEmployeeToTripService
    {
        private readonly IRepository<EmployeeToTrip> repository;

        public EmployeeToTripService(IRepository<EmployeeToTrip> repository)
        {
            this.repository = repository;
        }

        public Task<EmployeeToTrip> Add(EmployeeToTrip item)
        {
            return repository.Add(item);
        }
    }
}
