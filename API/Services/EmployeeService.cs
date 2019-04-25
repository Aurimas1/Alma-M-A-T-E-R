using API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> repository;

        public EmployeeService(IRepository<Employee> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Employee> GetAll()
        {
            return repository.GetAll();
        }
    }
}
