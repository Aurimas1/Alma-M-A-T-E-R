using API.Repositories;
using System.Collections.Generic;
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

        public async Task<Employee> Ensure(Employee employee)
        {
            var exists = repository.Get(e => e.Email == employee.Email);
            if (null == exists)
            {
                return await repository.Add(new Employee {Email = employee.Email, Name = employee.Name, Role = employee.Email == "aurimaiteo@gmail.com" ? Role.Admin : null });
            }
            else
            {
                return exists;
            }
        }
    }
}
